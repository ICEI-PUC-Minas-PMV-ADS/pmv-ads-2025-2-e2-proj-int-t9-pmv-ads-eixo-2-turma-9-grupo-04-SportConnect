using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Repositories;
using SportConnect.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace SportConnect.Controllers
{
    public class UsuariosController : Controller
    {
        public readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idClaim, out var id)) return id;
            return null;
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario == null)
                {
                    return NotFound();
                }

                var dados = _context.Usuarios.FirstOrDefault(c => c.Email == usuario.Email || c.Cpf == usuario.Cpf);

                if (dados == null)
                {
                    if (usuario.DataDeNascimento > DateOnly.FromDateTime(DateTime.Now))
                    {
                        ViewBag.Message = "Data inválida!";
                    }

                    else
                    {
                        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                        usuario.Cpf = usuario.Cpf.Replace(".", "").Replace("-", "");
                        _context.Usuarios.Add(usuario);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ViewBag.Message = "Conta já cadastrada!";
                }
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            if (usuario == null)
            {
                return NotFound();
            }

            var dados = _context.Usuarios.FirstOrDefault(c => c.Email == usuario.Email);

            if (dados == null)
            {
                ViewBag.Message = "Dados incorretos!";
                return View();
            }

            var senhaOk = BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha);

            if (senhaOk)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dados.Nome),
                    new Claim(ClaimTypes.NameIdentifier, dados.Id.ToString()),
                };

                var usuarioIdentify = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentify);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddHours(8),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, props);

                return RedirectToAction("Index", "Grupos");
            }
            else
            {
                ViewBag.Message = "Dados incorretos!";
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AlterarSenha()
        {
            return View();
        }

        public IActionResult ConfirmarSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarSenha(Usuario usuario)
        {
            if (usuario.Senha == null || usuario.Email == null)
            {
                return NotFound();
            }

            var dados = _context.Usuarios.FirstOrDefault(c => c.Email == usuario.Email);

            if (dados == null)
            {
                return RedirectToAction("Sucesso");
            }

            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            dados.Senha = senhaCriptografada;
            await _context.SaveChangesAsync();

            return RedirectToAction("Sucesso");
        }

        public IActionResult Sucesso()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                return RedirectToAction("AcessoNegado");
            }

            var dados = await _context.Usuarios.FindAsync(id);

            if (dados == null)
            {
                return NotFound();
            }

            dados.Cpf = Convert.ToUInt64(dados.Cpf).ToString(@"000\.000\.000\-00");

            switch (dados.Estado)
            {
                case "AC":
                    dados.Estado = "Acre";
                    break;
                case "AL":
                    dados.Estado = "Alagoas";
                    break;
                case "AP":
                    dados.Estado = "Amapá";
                    break;
                case "AM":
                    dados.Estado = "Amazonas";
                    break;
                case "BA":
                    dados.Estado = "Bahia";
                    break;
                case "CE":
                    dados.Estado = "Ceará";
                    break;
                case "DF":
                    dados.Estado = "Distrito Federal";
                    break;
                case "ES":
                    dados.Estado = "Espírito Santo";
                    break;
                case "GO":
                    dados.Estado = "Goiás";
                    break;
                case "MA":
                    dados.Estado = "Maranhão";
                    break;
                case "MT":
                    dados.Estado = "Mato Grosso";
                    break;
                case "MS":
                    dados.Estado = "Mato Grosso do Sul";
                    break;
                case "MG":
                    dados.Estado = "Minas Gerais";
                    break;
                case "PA":
                    dados.Estado = "Pará";
                    break;
                case "PB":
                    dados.Estado = "Paraíba";
                    break;
                case "PR":
                    dados.Estado = "Paraná";
                    break;
                case "PE":
                    dados.Estado = "Pernambuco";
                    break;
                case "PI":
                    dados.Estado = "Piauí";
                    break;
                case "RJ":
                    dados.Estado = "Rio de Janeiro";
                    break;
                case "RN":
                    dados.Estado = "Rio Grande do Norte";
                    break;
                case "RS":
                    dados.Estado = "Rio Grande do Sul";
                    break;
                case "RO":
                    dados.Estado = "Rondônia";
                    break;
                case "RR":
                    dados.Estado = "Roraima";
                    break;
                case "SC":
                    dados.Estado = "Santa Catarina";
                    break;
                case "SP":
                    dados.Estado = "São Paulo";
                    break;
                case "SE":
                    dados.Estado = "Sergipe";
                    break;
                case "TO":
                    dados.Estado = "Tocantins";
                    break;
            }

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(dados);
        }

        [Authorize]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                return RedirectToAction("AcessoNegado");
            }

            var dados = await _context.Usuarios.FindAsync(id);

            if (dados == null)
            {
                return NotFound();
            }

            dados.Cpf = Convert.ToUInt64(dados.Cpf).ToString(@"000\.000\.000\-00");

            var notificacoesNaoLidas = _context.Notificacoes
                    .Count(x => x.UsuarioId == GetCurrentUserId() && x.Lida == "Nao");

            ViewBag.NotificacoesCount = notificacoesNaoLidas.ToString();

            return View(dados);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            usuario.Cpf = usuario.Cpf.Replace("-", "").Replace(".", "");
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detalhes", new { id = usuario.Id });
        }

        [Authorize]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                return RedirectToAction("AcessoNegado");
            }

            var dados = await _context.Usuarios.FindAsync(id);

            if (dados == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(dados);
            await _context.SaveChangesAsync();
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}