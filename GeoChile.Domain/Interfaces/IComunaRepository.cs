using GeoChile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChile.Domain.Interfaces
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> GetComunasByRegionIdAsync(int idRegion);
        Task<IEnumerable<Comuna>> GetComunaByRegionIdAsync(int idRegion, int idComuna);
        Task UpdateAsync(Comuna comuna);
    }
}
