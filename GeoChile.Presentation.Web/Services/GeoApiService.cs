using GeoChile.Presentation.Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeoChile.Presentation.Web.Services
{
    public class GeoApiService: IGeoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeoApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginRequest = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseViewModel>();
                _httpContextAccessor.HttpContext.Session.SetString("JWToken", loginResponse.Token);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<RegionViewModel>> GetRegionesAsync()
        {
            AddAuthorizationHeader();
            // Opciones para ignorar la diferencia entre mayúsculas y minúsculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return await _httpClient.GetFromJsonAsync<IEnumerable<RegionViewModel>>("/api/region");
        }

        public async Task<IEnumerable<ComunaViewModel>> GetComunasPorRegionAsync(int idRegion)
        {
            AddAuthorizationHeader();
            // Opciones para ignorar la diferencia entre mayúsculas y minúsculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return await _httpClient.GetFromJsonAsync<IEnumerable<ComunaViewModel>>($"/api/region/{idRegion}/comuna");
        }

        public async Task<ComunaViewModel> GetComunaByIdRegionIdAsync(int idRegion, int idComuna)
        {
            AddAuthorizationHeader();
            // Opciones para ignorar la diferencia entre mayúsculas y minúsculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return await _httpClient.GetFromJsonAsync<ComunaViewModel>($"/api/region/{idRegion}/comuna/{idComuna}");
        }

        public async Task<bool> UpdateComunaAsync(ComunaViewModel comuna)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync($"/api/region/{comuna.IdRegion}/comuna", comuna);
            return response.IsSuccessStatusCode;
        }
    }
}
