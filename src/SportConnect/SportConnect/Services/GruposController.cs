using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportConnect\Controllers
{
    [Authorize]
    public class GruposController : Controller
    {
        private const string StatusInscrito = "Inscrito";

     
    }
}