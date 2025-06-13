using GeoChile.Presentation.Web.Models;

namespace GeoChile.Presentation.Web.Services
{
    public interface IGeoApiService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<IEnumerable<RegionViewModel>> GetRegionesAsync();
        Task<IEnumerable<ComunaViewModel>> GetComunasPorRegionAsync(int idRegion);
        Task<ComunaViewModel> GetComunaByIdRegionIdAsync(int idRegion,int idComuna); // Necesario para el formulario de edición
        Task<bool> UpdateComunaAsync(ComunaViewModel comuna);
    }
}
