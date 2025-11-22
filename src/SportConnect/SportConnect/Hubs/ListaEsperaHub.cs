using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SportConnect.Hubs
{
    public class ListaEsperaHub : Hub
    {
        
        public async Task AtualizarListaEspera(object dados)
        {
            await Clients.All.SendAsync("ReceberAtualizacaoLista", dados);
        }
    }
}