using GeoChile.Presentation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Presentation.Web.Controllers
{
    public class RegionController: Controller
    {
        private readonly IGeoApiService _apiService;
        public RegionController(IGeoApiService apiService) { _apiService = apiService; }

        public async Task<IActionResult> Index()
        {
            // Verifica si hay un token en la sesión. Si no, redirige al login.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                return RedirectToAction("Login", "Account");
            }

            var regiones = await _apiService.GetRegionesAsync();
            return View(regiones);
        }
    }
}
