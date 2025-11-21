using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportConnectControllers
{
    [Authorize]
    public class GruposController : Controller
    {
        private const string StatusInscrito = "Inscrito";

     
    }
}