using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportConnect.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
    
namespace SportConnect.Services
{
    public class ListaEsperaBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ListaEsperaBackgroundService> _logger;
        private readonly TimeSpan _interval;

        public ListaEsperaBackgroundService(IServiceProvider serviceProvider, ILogger<ListaEsperaBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _interval = TimeSpan.FromSeconds(30);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ListaEsperaBackgroundService iniciado.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessarFilas(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar filas de espera.");
                }

                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (OperationCanceledException) { break; }
            }
            _logger.LogInformation("ListaEsperaBackgroundService finalizado.");
        }

        private async Task ProcessarFilas(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var filaService = scope.ServiceProvider.GetRequiredService<IListaEsperaService>();

            var grupos = await db.Grupos
                .AsNoTracking()
                .Where(g => g.ListaEspera && g.NumeroMaximoParticipantes > 0)
                .Select(g => g.Id)
                .ToListAsync(ct);

            foreach (var grupoId in grupos)
            {
                try
                {
                    await filaService.PromoteNextIfVagaAsync(grupoId, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao promover próximo para o grupo {GrupoId}", grupoId);
                }
            }
        }
    }
}