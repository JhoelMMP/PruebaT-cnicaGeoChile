using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Application.Controllers
{
    [ApiController]
    [Route("api/PruebaDeVida")]
    public class PruebaDeVidaController : ControllerBase
    {
       
        [HttpGet]
        public string Get()
        {
            return "Estoy Vivo";
        }
    }
}
