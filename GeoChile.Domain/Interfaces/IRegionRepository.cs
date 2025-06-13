using GeoChile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChile.Domain.Interfaces
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetRegionAsync();
        Task<IEnumerable<Region>> GetRegionByIdAsync(int idRegion);
        //Task UpdateAsync(Region region);
    }
}
