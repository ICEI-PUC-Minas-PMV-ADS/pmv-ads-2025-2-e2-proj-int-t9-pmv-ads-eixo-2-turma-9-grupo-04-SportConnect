using CriarGrupo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SportConnect.Models;
using SportConnect.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;

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

            List<SelectListItem> items = new List<SelectListItem>();
            var modalidades = await _context.Modalidades.Select(m => m.Nome).ToListAsync();

            items.Add(new SelectListItem { Text = "Pesquisar modalidade", Value = "", Selected = true, Disabled = true });
            items.Add(new SelectListItem { Text = "Nenhuma", Value = "Nenhuma" });

            foreach (var modalidade in modalidades)
            {
                items.Add(new SelectListItem { Text = modalidade, Value = modalidade });
            }

            ViewBag.Modalidades = items;

            foreach (var dado in dados)
            {
                switch (dado.Estado)
                {
                    case "AC":
                        dado.Estado = "Acre";
                        break;
                    case "AL":
                        dado.Estado = "Alagoas";
                        break;
                    case "AP":
                        dado.Estado = "Amapá";
                        break;
                    case "AM":
                        dado.Estado = "Amazonas";
                        break;
                    case "BA":
                        dado.Estado = "Bahia";
                        break;
                    case "CE":
                        dado.Estado = "Ceará";
                        break;
                    case "DF":
                        dado.Estado = "Distrito Federal";
                        break;
                    case "ES":
                        dado.Estado = "Espírito Santo";
                        break;
                    case "GO":
                        dado.Estado = "Goiás";
                        break;
                    case "MA":
                        dado.Estado = "Maranhão";
                        break;
                    case "MT":
                        dado.Estado = "Mato Grosso";
                        break;
                    case "MS":
                        dado.Estado = "Mato Grosso do Sul";
                        break;
                    case "MG":
                        dado.Estado = "Minas Gerais";
                        break;
                    case "PA":
                        dado.Estado = "Pará";
                        break;
                    case "PB":
                        dado.Estado = "Paraíba";
                        break;
                    case "PR":
                        dado.Estado = "Paraná";
                        break;
                    case "PE":
                        dado.Estado = "Pernambuco";
                        break;
                    case "PI":
                        dado.Estado = "Piauí";
                        break;
                    case "RJ":
                        dado.Estado = "Rio de Janeiro";
                        break;
                    case "RN":
                        dado.Estado = "Rio Grande do Norte";
                        break;
                    case "RS":
                        dado.Estado = "Rio Grande do Sul";
                        break;
                    case "RO":
                        dado.Estado = "Rondônia";
                        break;
                    case "RR":
                        dado.Estado = "Roraima";
                        break;
                    case "SC":
                        dado.Estado = "Santa Catarina";
                        break;
                    case "SP":
                        dado.Estado = "São Paulo";
                        break;
                    case "SE":
                        dado.Estado = "Sergipe";
                        break;
                    case "TO":
                        dado.Estado = "Tocantins";
                        break;
                }
            }

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == uid && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            ViewBag.Espera = null;

            return View(dados);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var modalidades = await _context.Modalidades.Select(m => m.Nome).ToListAsync();

            items.Add(new SelectListItem { Text = "Procurar modalidade", Value = "", Selected = true, Disabled = true });

            foreach (var modalidade in modalidades)
            {
                items.Add(new SelectListItem { Text = modalidade, Value = modalidade });
            }

            ViewBag.Modalidades = items;

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            ViewBag.Espera = null;

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

            List<SelectListItem> items = new List<SelectListItem>();
            var modalidades = await _context.Modalidades.Select(m => m.Nome).ToListAsync();

            if (!string.IsNullOrEmpty(grupo.Modalidade) && !modalidades.Contains(grupo.Modalidade))
            {
                items.Add(new SelectListItem { Text = grupo.Modalidade, Value = grupo.Modalidade, Selected = true });
            }

            foreach (var modalidadeItem in modalidades)
            {
                if (modalidadeItem == grupo.Modalidade) items.Add(new SelectListItem { Text = modalidadeItem, Value = modalidadeItem, Selected = true });
                else items.Add(new SelectListItem { Text = modalidadeItem, Value = modalidadeItem });
            }

            ViewBag.Modalidades = items;

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            ViewBag.Espera = null;

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

            ViewBag.CurrentUserId = GetCurrentUserId();

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            ViewBag.Espera = null;

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

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Filtro(string nome, string estado, string cidade, string modalidade, string espera, string gruposDono, string gruposParticipo)
        {
            bool? boolean = null;
            List<Grupo> dados = new List<Grupo>();
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(nome)) nome = null;

            if (estado == "Selecione um estado" || estado == "Nenhum") estado = null;

            if (cidade == "Selecione uma cidade" || cidade == "Nenhuma") cidade = null;

            if (modalidade == "" || modalidade == "Nenhuma") modalidade = null;

            if (espera == "Tem lista de espera" || espera == "Nenhum") espera = null;
            else
            {
                if (espera == "Sim") boolean = true;
                else boolean = false;
            }

            if (gruposParticipo == null && gruposDono == null)
            {
                dados = await _context.Grupos
                .Where(g =>
                    (espera != null ? g.ListaEspera == boolean : true) &&
                    (estado != null ? g.Estado == estado : true) &&
                    (cidade != null ? g.Cidade == cidade : true) &&
                    (modalidade != null ? g.Modalidade == modalidade : true) &&
                    (nome != null ? g.Nome.Contains(nome) : true)
                )
                .ToListAsync();
            }
            else if (gruposDono != null)
            {
                if (!int.TryParse(idClaim, out var ownerId)) return Forbid();

                dados = await _context.Grupos
                    .Where(g =>
                        (g.UsuarioId == ownerId) &&
                        (espera != null ? g.ListaEspera == boolean : true) &&
                        (estado != null ? g.Estado == estado : true) &&
                        (cidade != null ? g.Cidade == cidade : true) &&
                        (modalidade != null ? g.Modalidade == modalidade : true) &&
                        (nome != null ? g.Nome.Contains(nome) : true)
                    )
                    .ToListAsync();
            }
            else
            {
                if (!int.TryParse(idClaim, out var uid)) return Forbid();

                var gruposId = await _context.Participacoes
                    .Where(p => p.UsuarioId == uid && p.StatusParticipacao == StatusInscrito)
                    .Select(p => p.GrupoId)
                    .ToListAsync();

                foreach (var grupoId in gruposId)
                {
                    var grupo = await _context.Grupos
                        .Where(g =>
                            (g.Id == grupoId) &&
                            (espera != null ? g.ListaEspera == boolean : true) &&
                            (estado != null ? g.Estado == estado : true) &&
                            (cidade != null ? g.Cidade == cidade : true) &&
                            (modalidade != null ? g.Modalidade == modalidade : true) &&
                            (nome != null ? g.Nome.Contains(nome) : true)
                        ).FirstOrDefaultAsync();

                    if (grupo != null) dados.Add(grupo);
                }
            }

            if (int.TryParse(idClaim, out var uidForView))
            {
                var meus = await _context.Participacoes
                    .Where(p => p.UsuarioId == uidForView && p.StatusParticipacao == StatusInscrito)
                    .Select(p => p.GrupoId)
                    .ToListAsync();

                ViewBag.MeusGrupos = new HashSet<int>(meus);
            }
            else
            {
                ViewBag.MeusGrupos = new HashSet<int>();
            }

            if (int.TryParse(idClaim, out uidForView))
            {
                var meusEmFila = await _context.Participacoes
                    .Where(p => p.UsuarioId == uidForView && p.StatusParticipacao == StatusFila)
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

            List<SelectListItem> items = new List<SelectListItem>();
            var modalidadesList = await _context.Modalidades.Select(m => m.Nome).ToListAsync();

            items.Add(new SelectListItem { Text = "Pesquisar modalidade", Value = "", Selected = true, Disabled = true });
            items.Add(new SelectListItem { Text = "Nenhuma", Value = "Nenhuma" });

            foreach (var modalidadeItem in modalidadesList)
            {
                items.Add(new SelectListItem { Text = modalidadeItem, Value = modalidadeItem });
            }

            ViewBag.Modalidades = items;

            if (!modalidadesList.Contains(modalidade) && modalidade != null) ViewBag.Text = "Sim";

            ViewBag.GruposLotados = gruposLotados;

            ViewBag.Estado = estado;
            ViewBag.Nome = nome;
            ViewBag.Modalidade = modalidade;
            ViewBag.Espera = espera;
            ViewBag.Cidade = cidade;
            ViewBag.GruposParticipo = gruposParticipo;
            ViewBag.GruposDono = gruposDono;

            foreach (var dado in dados)
            {
                switch (dado.Estado)
                {
                    case "AC":
                        dado.Estado = "Acre";
                        break;
                    case "AL":
                        dado.Estado = "Alagoas";
                        break;
                    case "AP":
                        dado.Estado = "Amapá";
                        break;
                    case "AM":
                        dado.Estado = "Amazonas";
                        break;
                    case "BA":
                        dado.Estado = "Bahia";
                        break;
                    case "CE":
                        dado.Estado = "Ceará";
                        break;
                    case "DF":
                        dado.Estado = "Distrito Federal";
                        break;
                    case "ES":
                        dado.Estado = "Espírito Santo";
                        break;
                    case "GO":
                        dado.Estado = "Goiás";
                        break;
                    case "MA":
                        dado.Estado = "Maranhão";
                        break;
                    case "MT":
                        dado.Estado = "Mato Grosso";
                        break;
                    case "MS":
                        dado.Estado = "Mato Grosso do Sul";
                        break;
                    case "MG":
                        dado.Estado = "Minas Gerais";
                        break;
                    case "PA":
                        dado.Estado = "Pará";
                        break;
                    case "PB":
                        dado.Estado = "Paraíba";
                        break;
                    case "PR":
                        dado.Estado = "Paraná";
                        break;
                    case "PE":
                        dado.Estado = "Pernambuco";
                        break;
                    case "PI":
                        dado.Estado = "Piauí";
                        break;
                    case "RJ":
                        dado.Estado = "Rio de Janeiro";
                        break;
                    case "RN":
                        dado.Estado = "Rio Grande do Norte";
                        break;
                    case "RS":
                        dado.Estado = "Rio Grande do Sul";
                        break;
                    case "RO":
                        dado.Estado = "Rondônia";
                        break;
                    case "RR":
                        dado.Estado = "Roraima";
                        break;
                    case "SC":
                        dado.Estado = "Santa Catarina";
                        break;
                    case "SP":
                        dado.Estado = "São Paulo";
                        break;
                    case "SE":
                        dado.Estado = "Sergipe";
                        break;
                    case "TO":
                        dado.Estado = "Tocantins";
                        break;
                }
            }

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View("~/Views/Grupos/Index.cshtml", dados);
        }

        public async Task<IActionResult> GerenciarParticipantes(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id.Value);
            if (grupo == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null || grupo.UsuarioId != userId.Value)
            {
                return Forbid();
            }

            var participantesIds = await _context.Participacoes
                .Where(p => p.GrupoId == id && p.StatusParticipacao == StatusInscrito)
                .Select(p => p.UsuarioId)
                .ToListAsync();

            var participantes = await _context.Usuarios
                .Where(u => participantesIds.Contains(u.Id))
                .ToListAsync();

            ViewBag.Participantes = participantes;

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(grupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverParticipante(int grupoId, int usuarioId)
        {
            var loggedInUserId = GetCurrentUserId();
            if (loggedInUserId == null) return Challenge();

            var grupo = await _context.Grupos.FindAsync(grupoId);
            if (grupo == null) return NotFound();

            if (grupo.UsuarioId != loggedInUserId.Value) return Forbid();

            var participacao = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId);

            if (participacao == null)
            {
                TempData["MensagemRemocao"] = "Participante não encontrado!";
                return RedirectToAction("GerenciarParticipantes", new { id = grupoId });
            }


            bool promoted = await _filaService.RemoveAndPromoteNextAsync(grupoId, usuarioId);

            if (_context.Participacoes.Any(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId))
            {
                _context.Participacoes.Remove(participacao);
                await _context.SaveChangesAsync();
            }

            string mensagem;
            if (promoted)
            {
                mensagem = "Participante removido! Próximo da fila inscrito.";
            }
            else
            {
                mensagem = "Participante removido!";
            }

            TempData["MensagemRemocao"] = mensagem;

            return RedirectToAction("GerenciarParticipantes", new { id = grupoId });
        }
public async Task<IActionResult> BaixarRelatorioPDF(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id.Value);
            if (grupo == null) return NotFound();

            var userId = GetCurrentUserId();
            if (userId == null || grupo.UsuarioId != userId.Value)
            {
                return Forbid();
            }

            var participantesIds = await _context.Participacoes
                .Where(p => p.GrupoId == id && p.StatusParticipacao == StatusInscrito)
                .Select(p => p.UsuarioId)
                .ToListAsync();

            var participantes = await _context.Usuarios
                .Where(u => participantesIds.Contains(u.Id))
                .ToListAsync();

            string caminhoImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo_relatorio.png");

            var relatorio = new RelatorioParticipantesPDF(grupo, participantes, caminhoImagem);

            byte[] pdfBytes = relatorio.GeneratePdf();

            string nomeArquivo = $"Relatorio_Participantes_{grupo.Nome.Replace(" ", "_")}.pdf";

            return File(pdfBytes, "application/pdf", nomeArquivo);
        }
    }

    public class RelatorioParticipantesPDF : IDocument
    {
        private readonly Grupo _grupo;
        private readonly List<Usuario> _participantes;
        private readonly string _caminhoLogo;

        public RelatorioParticipantesPDF(Grupo grupo, List<Usuario> participantes, string caminhoLogo)
        {
            _grupo = grupo;
            _participantes = participantes ?? new List<Usuario>();
            _caminhoLogo = caminhoLogo;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(ComposeHeader);

                    page.Content().Element(ComposeContent);

                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Relatório de Participantes")
                           .FontSize(20).SemiBold().FontColor(Colors.Black);

                        col.Item().PaddingTop(5).AlignCenter().Text("Este documento apresenta a lista de participantes inscritos no grupo detalhado abaixo.")
                           .FontSize(10).FontColor(Colors.Grey.Darken2);
                    });

                    if (!string.IsNullOrEmpty(_caminhoLogo) && System.IO.File.Exists(_caminhoLogo))
                    {
                        row.ConstantItem(60).AlignRight().Image(_caminhoLogo).FitArea();
                    }
                });

                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                column.Item().PaddingVertical(10);

                column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text(text => { text.Span("Grupo: ").SemiBold(); text.Span(_grupo.Nome); });
                        c.Item().Text(text => { text.Span("Modalidade: ").SemiBold(); text.Span(_grupo.Modalidade ?? "Não informada"); });
                        c.Item().Text(text => { text.Span("Descrição: ").SemiBold(); text.Span(_grupo.Descricao ?? "-"); });
                    });

                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text(text => { text.Span("Local: ").SemiBold(); text.Span($"{_grupo.Cidade} - {_grupo.Estado}"); });
                        c.Item().Text(text => { text.Span("Total Inscritos: ").SemiBold(); text.Span($"{_participantes.Count}"); });
                        c.Item().Text(text => { text.Span("Data do Relatório: ").SemiBold(); text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")); });
                    });
                });

                column.Item().PaddingBottom(20);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.ConstantItem(30).Text("");
                    row.RelativeItem().Text("Nome do Participante").Bold();
                });

                col.Item().LineHorizontal(0.5f).LineColor(Colors.Black);

                int index = 1;
                foreach (var user in _participantes)
                {
                    col.Item().PaddingVertical(2).Row(row =>
                    {
                        row.ConstantItem(30).Text($"{index}.");
                        row.RelativeItem().Text(user.Nome);
                    });

                    col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten3);
                    index++;
                }

                if (_participantes.Count == 0)
                {
                    col.Item().PaddingTop(10).Text("Nenhum participante inscrito no momento.").Italic().FontColor(Colors.Grey.Medium);
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.PaddingTop(10).Column(col =>
            {
                col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                col.Item().PaddingTop(5).Text("Informações extraídas da plataforma SportConnect")
                    .FontSize(9).FontColor(Colors.Grey.Medium);
            });
        }
    }
}