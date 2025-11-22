using CriarGrupo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportConnect.Models;
using System.Security.Claims;
using System.Data;
using SportConnect.Services;

namespace SportConnect.Controllers
{
    [Authorize]
    public class GruposController : Controller
    {
        private const string StatusInscrito = "Inscrito";
        private const string StatusCancelado = "Cancelado";
        private const string StatusFila = "Lista de Espera";

        private readonly AppDbContext _context;
        private readonly IListaEsperaService _filaService;

        public GruposController(AppDbContext context, IListaEsperaService filaService)
        {
            _context = context;
            _filaService = filaService;
        }

        private int? GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idClaim, out var id)) return id;
            return null;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dados = await _context.Grupos.AsNoTracking().ToListAsync();

            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idClaim, out var uid))
            {
                var meus = await _context.Participacoes
                    .Where(p => p.UsuarioId == uid && p.StatusParticipacao == StatusInscrito)
                    .Select(p => p.GrupoId)
                    .ToListAsync();

                ViewBag.MeusGrupos = new HashSet<int>(meus);
            }
            else
            {
                ViewBag.MeusGrupos = new HashSet<int>();
            }

            if (int.TryParse(idClaim, out uid))
            {
                var meusEmFila = await _context.Participacoes
                    .Where(p => p.UsuarioId == uid && p.StatusParticipacao == StatusFila)
                    .Select(p => p.GrupoId)
                    .ToListAsync();

                ViewBag.MeusGruposFila = new HashSet<int>(meusEmFila);
            }
            else
            {
                ViewBag.MeusGruposFila = new HashSet<int>();
            }

            var ativosPorGrupo = await _context.Participacoes
                .Where(p => p.StatusParticipacao == StatusInscrito)
                .GroupBy(p => p.GrupoId)
                .Select(g => new { GrupoId = g.Key, Qtde = g.Count() })
                .ToListAsync();

            var mapaQtde = ativosPorGrupo.ToDictionary(x => x.GrupoId, x => x.Qtde);
            ViewBag.ParticipantesAtivos = mapaQtde;

            var gruposLotados = new HashSet<int>(
                dados.Where(g =>
                    g.NumeroMaximoParticipantes > 0
                    && mapaQtde.TryGetValue(g.Id, out var q)
                    && q >= g.NumeroMaximoParticipantes
                )
                .Select(g => g.Id)
            );

            ViewBag.GruposLotados = gruposLotados;

            return View(dados);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grupo grupo)
        {
            if (!ModelState.IsValid) return View(grupo);

            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();

            grupo.UsuarioId = userId;

            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();
            if (grupo.UsuarioId != userId) return Forbid();

            return View(grupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grupo grupo)
        {
            if (id != grupo.Id) return NotFound();

            var original = await _context.Grupos.FindAsync(id);
            if (original == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();
            if (original.UsuarioId != userId) return Forbid();

            if (!ModelState.IsValid) return View(original);

            original.Nome = grupo.Nome;
            original.Descricao = grupo.Descricao;
            original.NumeroMaximoParticipantes = grupo.NumeroMaximoParticipantes;
            original.ListaEspera = grupo.ListaEspera;
            original.Modalidade = grupo.Modalidade;
            original.Estado = grupo.Estado;
            original.Cidade = grupo.Cidade;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dados = await _context.Grupos.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (dados == null) return NotFound();

            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();
            if (grupo.UsuarioId != userId) return Forbid();

            return View(grupo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();
            if (grupo.UsuarioId != userId) return Forbid();

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Participar(int id, bool? waitlist)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            var grupo = await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return NotFound();

            
            var existente = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == id && p.UsuarioId == uid && p.StatusParticipacao != StatusCancelado);

            if (existente != null)
            {
                var mensagemExistente = "Você já está inscrito ou na lista de espera deste grupo.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, mensagem = mensagemExistente, grupoId = id });
                TempData["ok"] = mensagemExistente;
                return RedirectToAction(nameof(Index));
            }

            
            var ativos = await _context.Participacoes
                .CountAsync(p => p.GrupoId == id && p.StatusParticipacao == StatusInscrito);

            
            if (grupo.NumeroMaximoParticipantes == 0 || ativos < grupo.NumeroMaximoParticipantes)
            {
                var part = new Participacao
                {
                    UsuarioId = uid,
                    GrupoId = id,
                    StatusParticipacao = StatusInscrito,
                    DataInscricao = DateTimeOffset.UtcNow
                };
                _context.Participacoes.Add(part);
                await _context.SaveChangesAsync();

                var msg = "Você foi inscrito no grupo.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, status = StatusInscrito, mensagem = msg, grupoId = id, inscritos = ativos + 1 });
                TempData["ok"] = msg;
                return RedirectToAction(nameof(Index));
            }

            
            if (grupo.ListaEspera)
            {
                var part = new Participacao
                {
                    UsuarioId = uid,
                    GrupoId = id,
                    StatusParticipacao = StatusFila,
                    DataInscricao = DateTimeOffset.UtcNow
                };
                _context.Participacoes.Add(part);
                await _context.SaveChangesAsync();

                
                await _filaService.EnqueueAsync(id, uid);

                var msg = "Você entrou na lista de espera.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, status = StatusFila, mensagem = msg, grupoId = id, inscritos = ativos });
                TempData["ok"] = msg;
                return RedirectToAction(nameof(Index));
            }

            // grupo lotado e sem lista
            var semLista = "Grupo lotado e sem opção de lista de espera.";
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, mensagem = semLista, grupoId = id });
            TempData["ok"] = semLista;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sair(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            var promoted = await _filaService.RemoveAndPromoteNextAsync(id, uid);

            if (promoted)
                TempData["ok"] = "Você saiu do grupo. O próximo da fila foi inscrito.";
            else
                TempData["ok"] = "Você saiu do grupo.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SairDaFila(int id) 
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            var part = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == id
                                       && p.UsuarioId == uid
                                       && p.StatusParticipacao == StatusFila);

            if (part != null)
            {
                part.StatusParticipacao = StatusCancelado;
                await _context.SaveChangesAsync();
                TempData["ok"] = "Você saiu da lista de espera.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}