using GeoChile.Domain.Entities;
using GeoChile.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChile.Infrastructure.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly string _connectionString;

        public RegionRepository(IConfiguration configuration)
        {
            // La cadena de conexión se inyectará desde la configuración de la API
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Region>> GetRegionAsync()
        {
            var region = new List<Region>();

            // Usamos 'using' para asegurar que la conexión se cierre automáticamente
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("sp_GetRegiones", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        region.Add(new Region
                        {
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            Nombre = reader["Region"].ToString(),
                        });
                    }
                }
            }
            return region;
        }

        public async Task<IEnumerable<Region>> GetRegionByIdAsync(int idRegion)
        {
            var region = new List<Region>();

            // Usamos 'using' para asegurar que la conexión se cierre automáticamente
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("sp_GetRegionPorId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IdRegion", idRegion);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        region.Add(new Region
                        {
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            Nombre = reader["Region"].ToString(),
                        });
                    }
                }
            }
            return region;
        }

        //public async Task UpdateAsync(Region region)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
