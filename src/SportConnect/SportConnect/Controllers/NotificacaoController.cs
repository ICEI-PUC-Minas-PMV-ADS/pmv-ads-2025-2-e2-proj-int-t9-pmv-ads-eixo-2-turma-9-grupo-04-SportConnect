using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportConnect.Models;
using System.Security.Claims;

namespace SportConnect.Controllers
{
    [Authorize]
    public class NotificacoesController : Controller
    {
        private readonly AppDbContext _context;

        public NotificacoesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                return Forbid();
            }

            var notificacoes = await _context.Notificacoes.Where(x => x.UsuarioId == id).OrderByDescending(x => x.DataEnvio).ToListAsync();

            if(notificacoes.Count() == 0)
            {
                ViewBag.Nulo = "Não há notificações no momento";
            }
            else
            {
                foreach (var notificacao in notificacoes)
                {
                    if (notificacao.Lida == "Nao")
                    {
                        notificacao.Lida = "Sim";
                    }
                }

                await _context.SaveChangesAsync();
            }   

            return View(notificacoes);
        }
    }
}