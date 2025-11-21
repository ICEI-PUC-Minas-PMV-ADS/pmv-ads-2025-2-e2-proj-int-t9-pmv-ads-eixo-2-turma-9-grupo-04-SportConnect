using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportConnect.Hubs;
using SportConnect.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportConnect.Services
{
    public interface IListaEsperaService
    {
        Task EnqueueAsync(int grupoId, int usuarioId, CancellationToken ct = default);
        Task<bool> RemoveAndPromoteNextAsync(int grupoId, int usuarioId, CancellationToken ct = default);
        Task PromoteNextIfVagaAsync(int grupoId, CancellationToken ct = default);
    }

    public class ListaEsperaService : IListaEsperaService
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<ListaEsperaHub> _hub;
        private readonly ILogger<ListaEsperaService> _logger;

        private const string StatusInscrito = "Inscrito";
        private const string StatusFila = "Lista de Espera";
        private const string StatusCancelado = "Cancelado";

        public ListaEsperaService(AppDbContext db, IHubContext<ListaEsperaHub> hub, ILogger<ListaEsperaService> logger)
        {
            _db = db;
            _hub = hub;
            _logger = logger;
        }

        /// <summary>
        /// Coloca o usuário na lista de espera do grupo (cria ou atualiza participação).
        /// </summary>
        public async Task EnqueueAsync(int grupoId, int usuarioId, CancellationToken ct = default)
        {
            var part = await _db.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId, ct);

            if (part == null)
            {
                part = new Participacao
                {
                    GrupoId = grupoId,
                    UsuarioId = usuarioId,
                    StatusParticipacao = StatusFila,
                    DataInscricao = DateTimeOffset.UtcNow
                };
                _db.Participacoes.Add(part);
            }
            else
            {
                if (part.StatusParticipacao == StatusInscrito)
                {
                    // já inscrito — nada a fazer
                    _logger.LogDebug("Usuario {UsuarioId} já inscrito no grupo {GrupoId}", usuarioId, grupoId);
                    return;
                }

                if (part.StatusParticipacao == StatusFila)
                {
                    // já na fila — atualiza timestamp para manter ordem desejada ou opcionalmente manter original
                    part.DataInscricao = DateTimeOffset.UtcNow;
                    _db.Participacoes.Update(part);
                }
                else
                {
                    // reentrar na fila
                    part.StatusParticipacao = StatusFila;
                    part.DataInscricao = DateTimeOffset.UtcNow;
                    _db.Participacoes.Update(part);
                }
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Usuario {UsuarioId} entrou na fila do grupo {GrupoId}", usuarioId, grupoId);
        }

        /// <summary>
        /// Remove o usuário inscrito (desistência) e, na mesma transação, promove o próximo da fila se houver vaga.
        /// Retorna true se alguém foi promovido.
        /// </summary>
        public async Task<bool> RemoveAndPromoteNextAsync(int grupoId, int usuarioId, CancellationToken ct = default)
        {
            // Transação com isolamento forte para reduzir race conditions
            using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            try
            {
                var part = await _db.Participacoes
                    .FirstOrDefaultAsync(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId && p.StatusParticipacao == StatusInscrito, ct);

                if (part == null)
                {
                    await tx.RollbackAsync(ct);
                    _logger.LogDebug("RemoveAndPromote: participação ativa não encontrada para Usuario {UsuarioId} no Grupo {GrupoId}", usuarioId, grupoId);
                    return false;
                }

                // marca como cancelado
                part.StatusParticipacao = StatusCancelado;
                _db.Participacoes.Update(part);
                await _db.SaveChangesAsync(ct);

                // recalcula ocupação
                var inscritos = await _db.Participacoes
                    .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusInscrito)
                    .CountAsync(ct);

                var grupo = await _db.Grupos.FindAsync(new object[] { grupoId }, ct);

                if (grupo != null && (grupo.NumeroMaximoParticipantes == 0 || inscritos < grupo.NumeroMaximoParticipantes))
                {
                    // encontra próximo da fila
                    var proximo = await _db.Participacoes
                        .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusFila)
                        .OrderBy(p => p.DataInscricao)
                        .FirstOrDefaultAsync(ct);

                    if (proximo != null)
                    {
                        proximo.StatusParticipacao = StatusInscrito;
                        _db.Participacoes.Update(proximo);
                        await _db.SaveChangesAsync(ct);

                        await tx.CommitAsync(ct);

                        // notifica usuário promovido (não bloqueante)
                        try
                        {
                            await _hub.Clients.User(proximo.UsuarioId.ToString())
                                .SendAsync("Promovido", new { GrupoId = grupoId, ParticipacaoId = proximo.Id }, ct);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Falha ao notificar usuario {UsuarioId} promovido no grupo {GrupoId}", proximo.UsuarioId, grupoId);
                        }

                        _logger.LogInformation("Promovido participante {ParticipacaoId} no grupo {GrupoId}", proximo.Id, grupoId);
                        return true;
                    }
                }

                await tx.CommitAsync(ct);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro em RemoveAndPromoteNextAsync para grupo {GrupoId} usuario {UsuarioId}", grupoId, usuarioId);
                try { await tx.RollbackAsync(ct); } catch { }
                throw;
            }
        }

        /// <summary>
        /// Promove o próximo da fila para inscrito se houver vaga (usa para background service).
        /// </summary>
        public async Task PromoteNextIfVagaAsync(int grupoId, CancellationToken ct = default)
        {
            using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            try
            {
                var inscritos = await _db.Participacoes
                    .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusInscrito)
                    .CountAsync(ct);

                var grupo = await _db.Grupos.FindAsync(new object[] { grupoId }, ct);

                if (grupo == null)
                {
                    await tx.RollbackAsync(ct);
                    return;
                }

                if (grupo.NumeroMaximoParticipantes > 0 && inscritos >= grupo.NumeroMaximoParticipantes)
                {
                    await tx.RollbackAsync(ct);
                    return; // sem vaga
                }

                var proximo = await _db.Participacoes
                    .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusFila)
                    .OrderBy(p => p.DataInscricao)
                    .FirstOrDefaultAsync(ct);

                if (proximo == null)
                {
                    await tx.RollbackAsync(ct);
                    return;
                }

                proximo.StatusParticipacao = StatusInscrito;
                _db.Participacoes.Update(proximo);
                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                try
                {
                    await _hub.Clients.User(proximo.UsuarioId.ToString())
                        .SendAsync("Promovido", new { GrupoId = grupoId, ParticipacaoId = proximo.Id }, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falha ao notificar usuario {UsuarioId} promovido no grupo {GrupoId}", proximo.UsuarioId, grupoId);
                }

                _logger.LogInformation("Promovido participante {ParticipacaoId} no grupo {GrupoId} (background/manual)", proximo.Id, grupoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro em PromoteNextIfVagaAsync para grupo {GrupoId}", grupoId);
                try { await tx.RollbackAsync(ct); } catch { }
                throw;
            }
        }
    }
}