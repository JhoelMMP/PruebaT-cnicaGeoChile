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
    public class ComunaRepository : IComunaRepository
    {
        private readonly string _connectionString;

        public ComunaRepository(IConfiguration configuration)
        {
            // La cadena de conexión se inyectará desde la configuración de la API
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Comuna>> GetComunasByRegionIdAsync(int idRegion)
        {
            var comunas = new List<Comuna>();

            // Usamos 'using' para asegurar que la conexión se cierre automáticamente
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("sp_GetComunasPorRegion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IdRegion", idRegion);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        comunas.Add(new Comuna
                        {
                            IdComuna = Convert.ToInt32(reader["IdComuna"]),
                            Nombre = reader["Comuna"].ToString(),
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            InformacionAdicional = reader["InformacionAdicional"].ToString()
                        });
                    }
                }
            }
            return comunas;
        }

        public async Task<IEnumerable<Comuna>> GetComunaByRegionIdAsync(int idRegion, int idComuna)
        {
            var comunas = new List<Comuna>();

            // Usamos 'using' para asegurar que la conexión se cierre automáticamente
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("sp_GetComunaPorIdRegionId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IdRegion", idRegion);
                command.Parameters.AddWithValue("@IdComuna", idComuna);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        comunas.Add(new Comuna
                        {
                            IdComuna = Convert.ToInt32(reader["IdComuna"]),
                            Nombre = reader["Comuna"].ToString(),
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            InformacionAdicional = reader["InformacionAdicional"].ToString()
                        });
                    }
                }
            }
            return comunas;
        }

        public async Task UpdateAsync(Comuna comuna)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("sp_ActualizarComuna", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Agregamos los parámetros que espera el Stored Procedure
                command.Parameters.AddWithValue("@IdComuna", comuna.IdComuna);
                command.Parameters.AddWithValue("@IdRegion", comuna.IdRegion);
                command.Parameters.AddWithValue("@Comuna", comuna.Nombre);

                // El parámetro XML se debe tratar de forma especial
                SqlParameter xmlParam = new SqlParameter("@InformacionAdicional", SqlDbType.Xml)
                {
                    Value = comuna.InformacionAdicional
                };
                command.Parameters.Add(xmlParam);

                // ExecuteNonQueryAsync se usa para comandos que no devuelven resultados (INSERT, UPDATE, DELETE)
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
