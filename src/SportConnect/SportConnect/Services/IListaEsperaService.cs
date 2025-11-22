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
}