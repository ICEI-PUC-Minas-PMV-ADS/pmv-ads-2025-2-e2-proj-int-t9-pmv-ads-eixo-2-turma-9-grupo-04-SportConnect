using CriarGrupo.Models;
using Microsoft.AspNetCore.Authorization;        
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportConnect.Models;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CriarGrupo.Controllers
{
    [Authorize] 
    public class GruposController : Controller
    {
        private const string StatusInscrito = "Inscrito";
        private const string StatusCancelado = "Cancelado";

        private readonly AppDbContext _context;

        public GruposController(AppDbContext context)
        {
            _context = context;
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
            // 1) Carrega os grupos
            var dados = await _context.Grupos.AsNoTracking().ToListAsync();

            // 2) Carrega "meus grupos" (em que o usuário está INSCRITO)
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

            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            // 2b) NOVO BLOCO: carrega "meus grupos em FILA" (Lista de Espera)
            if (int.TryParse(idClaim, out uid))
            {
                var meusEmFila = await _context.Participacoes
                    .Where(p => p.UsuarioId == uid && p.StatusParticipacao == "Lista de Espera")
                    .Select(p => p.GrupoId)
                    .ToListAsync();

                ViewBag.MeusGruposFila = new HashSet<int>(meusEmFila);
            }
            else
            {
                ViewBag.MeusGruposFila = new HashSet<int>();
            }
            // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            // 3) Quantidade de participantes ATIVOS (Status = "Inscrito") por grupo
            var ativosPorGrupo = await _context.Participacoes
                .Where(p => p.StatusParticipacao == StatusInscrito)
                .GroupBy(p => p.GrupoId)
                .Select(g => new { GrupoId = g.Key, Qtde = g.Count() })
                .ToListAsync();

            // Mapa: GrupoId -> Qtde de inscritos
            var mapaQtde = ativosPorGrupo.ToDictionary(x => x.GrupoId, x => x.Qtde);

            // Disponibiliza o mapa para a view (caso queira exibir contadores depois)
            ViewBag.ParticipantesAtivos = mapaQtde;

            // 4) Quais grupos estão LOTADOS (qtde >= NumeroMaximoParticipantes e limite > 0)
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

            ViewBag.CurrentUserId = GetCurrentUserId();

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
        public async Task<IActionResult> Participar(int id, bool? waitlist) // waitlist vem da View quando grupo está lotado
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            // 1) Verifica se o grupo existe
            var grupo = await _context.Grupos.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return NotFound();

            // 2) Calcula lotação atual (apenas "Inscrito")
            var inscritos = await _context.Participacoes
                .CountAsync(p => p.GrupoId == id && p.StatusParticipacao == StatusInscrito);

            var temLimite = grupo.NumeroMaximoParticipantes > 0;
            var lotado = temLimite && inscritos >= grupo.NumeroMaximoParticipantes;

            // 3) Busca participação existente do usuário (pode reativar/alternar status)
            var part = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == id && p.UsuarioId == uid);

            // 4) Se está lotado...
            if (lotado)
            {
                // ... e o grupo NÃO aceita lista de espera
                if (!grupo.ListaEspera)
                {
                    TempData["ok"] = "Grupo lotado. Lista de espera não disponível.";
                    return RedirectToAction(nameof(Index));
                }

                // ... aceita lista de espera. Só entra na fila se a View pedir explicitamente
                var querFila = waitlist == true;

                if (!querFila)
                {
                    // veio sem a intenção de fila — só informa
                    TempData["ok"] = "Grupo lotado. Você pode entrar na lista de espera.";
                    return RedirectToAction(nameof(Index));
                }

                // aqui: usuário quer entrar na lista de espera
                if (part == null)
                {
                    part = new Participacao
                    {
                        GrupoId = id,
                        UsuarioId = uid,
                        StatusParticipacao = "Lista de Espera"
                    };
                    _context.Participacoes.Add(part);
                    TempData["ok"] = "Você entrou na lista de espera.";
                }
                else
                {
                    // Se já existe, atualiza para fila
                    if (part.StatusParticipacao != "Lista de Espera")
                    {
                        part.StatusParticipacao = "Lista de Espera";
                        TempData["ok"] = "Você entrou na lista de espera.";
                    }
                    else
                    {
                        TempData["ok"] = "Você já está na lista de espera.";
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 5) Não está lotado => INSCRITO
            if (part == null)
            {
                part = new Participacao
                {
                    GrupoId = id,
                    UsuarioId = uid,
                    StatusParticipacao = StatusInscrito
                };
                _context.Participacoes.Add(part);
                TempData["ok"] = "Inscrição realizada!";
            }
            else
            {
                if (part.StatusParticipacao != StatusInscrito)
                {
                    part.StatusParticipacao = StatusInscrito; // reativa ou tira da fila
                    TempData["ok"] = "Inscrição realizada!";
                }
                else
                {
                    TempData["ok"] = "Você já está inscrito neste grupo.";
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sair(int id) // id = GrupoId
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            // procura uma participação ATIVA (Inscrito) do usuário nesse grupo
            var part = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == id
                                       && p.UsuarioId == uid
                                       && p.StatusParticipacao == StatusInscrito);

            if (part != null)
            {
                // Soft delete: apenas muda o status
                part.StatusParticipacao = StatusCancelado;
                await _context.SaveChangesAsync();
                TempData["ok"] = "Você saiu do grupo.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SairDaFila(int id) // id = GrupoId
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Usuarios");

            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidStr, out var uid))
                return Forbid();

            // procura uma participação em lista de espera
            var part = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == id
                                       && p.UsuarioId == uid
                                       && p.StatusParticipacao == "Lista de Espera");

            if (part != null)
            {
                // Soft delete: apenas muda o status
                part.StatusParticipacao = StatusCancelado;
                await _context.SaveChangesAsync();
                TempData["ok"] = "Você saiu da lista de espera.";
            }

            return RedirectToAction(nameof(Index));
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
                .Where(p => p.GrupoId == id && p.StatusParticipacao == "Inscrito")
                .Select(p => p.UsuarioId)
                .ToListAsync();

            var participantes = await _context.Usuarios
                .Where(u => participantesIds.Contains(u.Id))
                .ToListAsync();

            ViewBag.Participantes = participantes;

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

            if (grupo.UsuarioId != loggedInUserId.Value)
            {
                return Forbid();
            }

            var participacao = await _context.Participacoes
                .FirstOrDefaultAsync(p => p.GrupoId == grupoId && p.UsuarioId == usuarioId);

            if (participacao != null)
            {
                _context.Participacoes.Remove(participacao);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Participante removido com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Participante não encontrado.";
            }

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
                .Where(p => p.GrupoId == id && p.StatusParticipacao == "Inscrito")
                .Select(p => p.UsuarioId)
                .ToListAsync();

            var participantes = await _context.Usuarios
                .Where(u => participantesIds.Contains(u.Id))
                .ToListAsync();

            var relatorio = new RelatorioParticipantesPDF(grupo, participantes);

            byte[] pdfBytes = relatorio.GeneratePdf();

            string nomeArquivo = $"Relatorio_Participantes_{grupo.Nome.Replace(" ", "_")}.pdf";

            return File(pdfBytes, "application/pdf", nomeArquivo);
        }

    }
}

    public class RelatorioParticipantesPDF : IDocument
    {
        private readonly Grupo _grupo;
        private readonly List<Usuario> _participantes;

        public RelatorioParticipantesPDF(Grupo grupo, List<Usuario> participantes)
        {
            _grupo = grupo;
            _participantes = participantes;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text($"Relatório de Participantes: {_grupo.Nome}")
                        .SemiBold().FontSize(20);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Spacing(5);

                            col.Item().Text("Nome").Bold();

                            foreach (var user in _participantes)
                            {
                                col.Item().Text(user.Nome);
                                col.Item().LineHorizontal(0.5f);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                        });
                });
        }
    }