using GeoChile.Application.DTOs;
using GeoChile.Domain.Entities;
using GeoChile.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Application.Controllers
{
    [ApiController]
    [Route("api/region")]
    public class RegionesController: ControllerBase
    {
        private readonly IComunaRepository _comunaRepository;
        private readonly IRegionRepository _regionRepository;

        public RegionesController(IComunaRepository comunaRepository, IRegionRepository regionRepository)
        {
            _comunaRepository = comunaRepository;
            _regionRepository = regionRepository;
        }

        // GET: api/region
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegiones()
        {
            //ESTO SE PUEDE DESCOMENTAR PARA PROBAR LA IMPLEMENTACION DE SERILOG
            //throw new Exception("¡Este es un error de prueba para verificar el logging y middleware!");

            var regiones = await _regionRepository.GetRegionAsync();

            // Mapeamos la entidad de Dominio a un DTO para la respuesta
            var regionesDto = regiones.Select(c => new RegionDTO
            {
                IdRegion = c.IdRegion,
                Nombre = c.Nombre,
            });

            if (regionesDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(regionesDto);
        }

        // GET: api/region/5
        [HttpGet("{idRegion}")]
        [Authorize]
        public async Task<ActionResult<RegionDTO>> GetRegionById(int idRegion)
        {
            var region = await _regionRepository.GetRegionByIdAsync(idRegion);

            // Mapeamos la entidad de Dominio a un DTO para la respuesta
            var regionDto = region.Select(c => new RegionDTO
            {
                IdRegion = c.IdRegion,
                Nombre = c.Nombre,
            });

            if (regionDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(regionDto.FirstOrDefault());
        }

        // GET: api/region/5/comuna
        [HttpGet("{idRegion}/comuna")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ComunaDTO>>> GetComunasDeRegion(int idRegion)
        {
            var comunas = await _comunaRepository.GetComunasByRegionIdAsync(idRegion);

            // Mapeamos la entidad de Dominio a un DTO para la respuesta
            var comunasDto = comunas.Select(c => new ComunaDTO
            {
                IdComuna = c.IdComuna,
                Nombre = c.Nombre,
                IdRegion = c.IdRegion,
                InformacionAdicional = c.InformacionAdicional
            });
            if (comunasDto.Count() == 0)
            {
                return NotFound();
            }
            return Ok(comunasDto);
        }

        // GET: api/region/5/comuna/1
        [HttpGet("{idRegion}/comuna/{idComuna}")]
        [Authorize]
        public async Task<ActionResult<ComunaDTO>> GetRegionByIdComunaById(int idRegion, int idComuna)
        {
            var comunas = await _comunaRepository.GetComunaByRegionIdAsync(idRegion, idComuna);

            // Mapeamos la entidad de Dominio a un DTO para la respuesta
            var comunasDto = comunas.Select(c => new ComunaDTO
            {
                IdComuna = c.IdComuna,
                Nombre = c.Nombre,
                IdRegion = c.IdRegion,
                InformacionAdicional = c.InformacionAdicional
            });

            if (comunasDto.Count() == 0)
            {
                return NotFound();
            }
            return Ok(comunasDto.FirstOrDefault());
        }

        // POST: api/region/5/comuna
        [HttpPost("{idRegion}/comuna")]
        [Authorize]
        public async Task<IActionResult> ActualizarComuna(int idRegion, [FromBody] ComunaDTO comunaDto)
        {
            if (idRegion != comunaDto.IdRegion)
            {
                return BadRequest("El IdRegion de la URL no coincide con el del cuerpo de la solicitud.");
            }

            // Mapeamos el DTO de entrada a una entidad de Dominio para pasarla al repositorio
            var comunaAActualizar = new Comuna
            {
                IdComuna = comunaDto.IdComuna,
                Nombre = comunaDto.Nombre,
                IdRegion = comunaDto.IdRegion,
                InformacionAdicional = comunaDto.InformacionAdicional
            };

            await _comunaRepository.UpdateAsync(comunaAActualizar);

            // HTTP 204 No Content es una respuesta estándar para una actualización exitosa sin devolver datos
            return NoContent();
        }

    }
}
