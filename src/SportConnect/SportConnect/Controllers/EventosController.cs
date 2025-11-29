using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportConnect.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportConnect.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out var id) ? id : null;
        }

        // LISTA EVENTOS DO GRUPO
        public async Task<IActionResult> Index(int grupoId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();

            var grupo = await _context.Grupos.FindAsync(grupoId);
            if (grupo == null) return NotFound();

            bool isCreator = grupo.UsuarioId == userId;
            bool isParticipant = await _context.Participacoes
                .AnyAsync(p => p.GrupoId == grupoId && p.UsuarioId == userId && p.StatusParticipacao == "Inscrito");

            if (!isCreator && !isParticipant)
            {
                return Forbid();
            }
            var eventos = await _context.Eventos
                .Where(e => e.GrupoId == grupoId)
                .ToListAsync();

            ViewBag.GrupoId = grupoId;

            ViewBag.CriadorDoGrupoId = grupo.UsuarioId;

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View("Eventos", eventos);
        }

        // FORMULÁRIO DE CRIAÇÃO
        public async Task<IActionResult> Create(int grupoId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();

            var grupo = await _context.Grupos.FindAsync(grupoId);
            if (grupo == null) return NotFound();

            // só o criador do grupo pode criar eventos
            if (grupo.UsuarioId != userId) return Forbid();

            var model = new Evento { GrupoId = grupoId };

            ViewBag.GrupoId = grupoId;

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(model);
        }

        // RECEBE O POST
        [HttpPost]
        public async Task<IActionResult> Create(int grupoId, Evento evento)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Challenge();

            var grupo = await _context.Grupos.FindAsync(grupoId);
            if (grupo == null) return NotFound();

            if (grupo.UsuarioId != userId) return Forbid();

            // seta FK automaticamente
            evento.GrupoId = grupoId;
            evento.CriadorId = userId.Value;

            // geocodificação
            // monta o endereço completo
            //string enderecoCompleto = $"{evento.Rua}, {evento.Numero}, {evento.Bairro}, {evento.Cidade}, ";
            var enderecoCompleto = $"{evento.Rua}, ${evento.Numero}, ${evento.Bairro},${evento.Cidade},MG, Brasil";

            // faz geocodificação
            var (lat, lng) = await GetCoordinatesFromOpenCage(enderecoCompleto);
            ;
            evento.Latitude = lat;
            evento.Longitude = lng;

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { grupoId = evento.GrupoId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            var userId = GetCurrentUserId();
            if (evento.CriadorId != userId) return Forbid();

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(evento);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            var original = await _context.Eventos.FindAsync(id);
            if (original == null) return NotFound();

            var userId = GetCurrentUserId();
            if (original.CriadorId != userId) return Forbid();

            // atualiza campos
            original.Nome = evento.Nome;
            original.Descricao = evento.Descricao;
            original.Cidade = evento.Cidade;
            original.Bairro = evento.Bairro;
            original.Rua = evento.Rua;
            original.Numero = evento.Numero;
            original.DataEvento = evento.DataEvento;


            // string enderecoCompleto = $"{evento.Rua}, {evento.Numero}, {evento.Bairro}, {evento.Cidade}";
            var enderecoCompleto = $"{evento.Rua}, ${evento.Numero}, ${evento.Bairro},${evento.Cidade},MG, Brasil";
            var (lat, lng) = await GetCoordinatesFromOpenCage(enderecoCompleto);

            original.Latitude = lat;
            original.Longitude = lng;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { grupoId = original.GrupoId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            var userId = GetCurrentUserId();
            if (evento.CriadorId != userId) return Forbid();

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            var userId = GetCurrentUserId();
            if (evento.CriadorId != userId) return Forbid();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { grupoId = evento.GrupoId });
        }

        private async Task<(double lat, double lng)> GetCoordinatesFromOpenCage(string address)
        {
            string apiKey = "d22bff7a5feb475e9bc22789f51e6a89";
            string url = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(address)}&key={apiKey}&language=pt&countrycode=br&limit=1";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return (0, 0);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var results = doc.RootElement.GetProperty("results");
            if (results.GetArrayLength() == 0) return (0, 0);

            var geometry = results[0].GetProperty("geometry");
            return (geometry.GetProperty("lat").GetDouble(), geometry.GetProperty("lng").GetDouble());
        }
    }
}
