using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChile.Domain.Entities
{
    public class Comuna
    {
        public int IdComuna { get; set; }
        public string? Nombre { get; set; } // El nombre de la comuna 
        public string? InformacionAdicional { get; set; } // El campo XML como string 
        public int IdRegion { get; set; }
    }
}
