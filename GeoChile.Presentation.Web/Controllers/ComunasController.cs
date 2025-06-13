using GeoChile.Presentation.Web.Models;
using GeoChile.Presentation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Presentation.Web.Controllers
{
    public class ComunasController: Controller
    {
        private readonly IGeoApiService _apiService;
        public ComunasController(IGeoApiService apiService) { _apiService = apiService; }

        // El 'id' aquí es el id de la Región
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.IdRegion = id; // Guardamos el Id de la región para la navegación
            var comunas = await _apiService.GetComunasPorRegionAsync(id);
            return View(comunas);
        }

        // GET para mostrar el formulario de edición
        public async Task<IActionResult> Editar(int idRegion,int idComuna) // id de la Comuna
        {
            var comuna = await _apiService.GetComunaByIdRegionIdAsync(idRegion, idComuna);
            if (comuna == null) return NotFound();
            return View(comuna);
        }

        // POST para procesar la edición
        [HttpPost]
        public async Task<IActionResult> Editar(ComunaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _apiService.UpdateComunaAsync(model);
            return RedirectToAction("Index", new { id = model.IdRegion });
        }
    }
}
