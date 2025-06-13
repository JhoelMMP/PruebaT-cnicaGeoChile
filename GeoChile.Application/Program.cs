using GeoChile.Application.Middleware;
using GeoChile.Application.Services;
using GeoChile.Domain.Interfaces;
using GeoChile.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console() // Escribe en la consola
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Escribe en un archivo, creando uno nuevo cada día
    .CreateLogger();

try
{
    Log.Information("Iniciando la aplicación...");
    var builder = WebApplication.CreateBuilder(args);

    //Usar Serilog para el logging de la aplicación
    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddScoped<IComunaRepository, ComunaRepository>();
    builder.Services.AddScoped<IRegionRepository, RegionRepository>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // 3. Añadir nuestro middleware de manejo de errores (lo crearemos en el siguiente paso)
    app.UseMiddleware<ExceptionHandlerMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
