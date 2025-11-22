#nullable enable
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportConnect.Models;
using SportConnect.Hubs;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportConnect.Services
{
    public class ListaEsperaService : IListaEsperaService
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<ListaEsperaHub>? _hub;
        private readonly ILogger<ListaEsperaService> _logger;

        private const string StatusInscrito = "Inscrito";
        private const string StatusFila = "Lista de Espera";
        private const string StatusCancelado = "Cancelado";

        public ListaEsperaService(AppDbContext db, IHubContext<ListaEsperaHub>? hub, ILogger<ListaEsperaService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _hub = hub;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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
                    _logger.LogDebug("Usuario {UsuarioId} já inscrito no grupo {GrupoId}", usuarioId, grupoId);
                    return;
                }

                if (part.StatusParticipacao == StatusFila)
                {
                    part.DataInscricao = DateTimeOffset.UtcNow;
                    _db.Participacoes.Update(part);
                }
                else
                {
                    part.StatusParticipacao = StatusFila;
                    part.DataInscricao = DateTimeOffset.UtcNow;
                    _db.Participacoes.Update(part);
                }
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Usuario {UsuarioId} entrou na fila do grupo {GrupoId}", usuarioId, grupoId);
        }

        public async Task<bool> RemoveAndPromoteNextAsync(int grupoId, int usuarioId, CancellationToken ct = default)
        {
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

                part.StatusParticipacao = StatusCancelado;
                _db.Participacoes.Update(part);
                await _db.SaveChangesAsync(ct);

                var inscritos = await _db.Participacoes
                    .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusInscrito)
                    .CountAsync(ct);

                var grupo = await _db.Grupos.FindAsync(new object[] { grupoId }, ct);

                if (grupo != null && (grupo.NumeroMaximoParticipantes == 0 || inscritos < grupo.NumeroMaximoParticipantes))
                {
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

                        try
                        {
                            if (_hub != null)
                            {
                                await _hub.Clients.User(proximo.UsuarioId.ToString())
                                    .SendAsync("Promovido", new { GrupoId = grupoId, ParticipacaoId = proximo.Id }, ct);
                            }
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

        public async Task<bool> RemoveAndPromoteNextAsync(int grupoId, int usuarioId, CancellationToken ct = default)
        {
            using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            try
            {
                var part = await _db.Participacoes
                    .FirstOrDefaultAsync(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId, ct);

                if (part == null)
                {
                    await tx.RollbackAsync(ct);
                    _logger.LogDebug("RemoveAndPromote: participação não encontrada para Usuario {UsuarioId} no Grupo {GrupoId}", usuarioId, grupoId);
                    return false;
                }

                // Atualiza para cancelado
                part.StatusParticipacao = StatusCancelado;
                _db.Participacoes.Update(part);
                await _db.SaveChangesAsync(ct);

                // Verifica vagas disponíveis
                var inscritos = await _db.Participacoes
                    .Where(p => p.GrupoId == grupoId && p.StatusParticipacao == StatusInscrito)
                    .CountAsync(ct);

                var grupo = await _db.Grupos.FindAsync(new object[] { grupoId }, ct);

                if (grupo != null && (grupo.NumeroMaximoParticipantes == 0 || inscritos < grupo.NumeroMaximoParticipantes))
                {
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

                        try
                        {
                            if (_hub != null)
                            {
                                await _hub.Clients.User(proximo.UsuarioId.ToString())
                                    .SendAsync("Promovido", new { GrupoId = grupoId, ParticipacaoId = proximo.Id }, ct);
                            }
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
    }
}