using GeoChile.Presentation.Web.Models;
using GeoChile.Presentation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Presentation.Web.Controllers
{
    public class AccountController: Controller
    {
        private readonly IGeoApiService _apiService;
        public AccountController(IGeoApiService apiService) { _apiService = apiService; }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool success = await _apiService.LoginAsync(model.Username, model.Password);
            if (success)
            {
                return RedirectToAction("Index", "Region");
            }
            ModelState.AddModelError(string.Empty, "Login fallido.");
            return View(model);
        }
    }
}
