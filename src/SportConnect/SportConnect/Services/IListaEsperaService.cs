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
        
        public Task EnqueueAsync(int grupoId, int usuarioId, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<bool> RemoveAndPromoteNextAsync(int grupoId, int usuarioId, CancellationToken ct = default) => throw new NotImplementedException();
        public Task PromoteNextIfVagaAsync(int grupoId, CancellationToken ct = default) => throw new NotImplementedException();
    }
}

builder.Services.AddScoped<IListaEsperaService, ListaEsperaService>();