🚀 Solución Prueba Técnica - GeoChile
Este repositorio contiene la solución completa para la prueba técnica de desarrollador. La aplicación permite gestionar Regiones y Comunas de Chile, siguiendo una arquitectura limpia, segura y escalable.





📋 Tabla de Contenidos
Tecnologías Utilizadas
Prerrequisitos
Guía de Instalación y Configuración
Paso 1: Clonar el Repositorio
Paso 2: Levantar la Base de Datos con Docker
Paso 3: Configurar las Aplicaciones
Paso 4: Ejecutar la Solución
Uso de la Aplicación
Arquitectura del Proyecto
✨ Tecnologías Utilizadas
Backend: ASP.NET Core Web API (.NET 8)
Frontend: ASP.NET Core MVC (.NET 8)
Base de Datos: SQL Server 2022 
Contenerización: Docker Desktop
Arquitectura: Diseño Guiado por el Dominio (DDD) en 4 capas. 

Autenticación: Basada en Tokens (JWT).
Acceso a Datos: ADO.NET con uso exclusivo de Procedimientos Almacenados. 
Logging: Serilog
Estilos: Bootstrap 5
🔧 Prerrequisitos
Antes de comenzar, asegúrate de tener instalado el siguiente software:

.NET SDK 8.0 o superior.
Docker Desktop.
Un IDE como Visual Studio 2022 o VS Code.
Una herramienta para gestionar la base de datos como Azure Data Studio o SSMS.
⚙️ Guía de Instalación y Configuración
Sigue estos pasos para levantar el entorno completo.

Paso 1: Clonar el Repositorio
Bash

git clone https://URL-DE-TU-REPOSITORIO.git
cd NOMBRE-DE-LA-CARPETA
Paso 2: Levantar la Base de Datos con Docker
Asegúrate de que Docker Desktop esté en ejecución en tu máquina.

Abre una terminal (PowerShell, CMD, etc.) y ejecuta el siguiente comando para crear un contenedor de SQL Server:

Bash

docker run --name sql-server-prueba -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuClaveSegura123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
🔑 Nota: Reemplaza TuClaveSegura123! por una contraseña segura de tu elección.

Conéctate al contenedor usando tu gestor de base de datos preferido con las siguientes credenciales:

Servidor: localhost
Usuario: sa
Contraseña: La que elegiste en el paso anterior.
Ejecuta el siguiente script SQL para crear la base de datos, las tablas y los procedimientos almacenados necesarios.

&lt;details>
&lt;summary>Haga clic para ver el Script SQL completo&lt;/summary>

SQL

-- Creación de la Base de Datos
CREATE DATABASE GeoChileDB;
GO

USE GeoChileDB;
GO

-- Validar y eliminar tablas si ya existen
DROP TABLE IF EXISTS dbo.Comuna;
GO
DROP TABLE IF EXISTS dbo.Region;
GO

-- Creación de Tablas
CREATE TABLE Region (
    IdRegion INT PRIMARY KEY IDENTITY(1,1),
    Region NVARCHAR(128) NOT NULL
);
GO

CREATE TABLE Comuna (
    IdComuna INT PRIMARY KEY IDENTITY(1,1),
    IdRegion INT FOREIGN KEY REFERENCES Region(IdRegion),
    Comuna NVARCHAR(128) NOT NULL,
    InformacionAdicional XML NULL
);
GO

-- Creación de Procedimientos Almacenados
CREATE PROCEDURE sp_GetRegiones
AS BEGIN SELECT IdRegion, Region FROM Region; END
GO

CREATE PROCEDURE sp_GetComunasPorRegion @IdRegion INT
AS BEGIN SELECT IdComuna, IdRegion, Comuna, InformacionAdicional FROM Comuna WHERE IdRegion = @IdRegion; END
GO

CREATE PROCEDURE sp_GetComunaById @IdComuna INT
AS BEGIN SELECT IdComuna, IdRegion, Comuna, InformacionAdicional FROM Comuna WHERE IdComuna = @IdComuna; END
GO

CREATE PROCEDURE sp_ActualizarComuna
    @IdComuna INT,
    @IdRegion INT,
    @Comuna NVARCHAR(128),
    @InformacionAdicional XML
AS
BEGIN
    SET NOCOUNT ON;
    MERGE INTO Comuna AS Target
    USING (VALUES (@IdComuna, @IdRegion, @Comuna, @InformacionAdicional))
    AS Source (IdComuna, IdRegion, Comuna, InformacionAdicional)
    ON Target.IdComuna = Source.IdComuna
    WHEN MATCHED THEN
        UPDATE SET
            Comuna = Source.Comuna,
            InformacionAdicional = Source.InformacionAdicional,
            IdRegion = Source.IdRegion;
END
GO

-- Inserción de Datos de Prueba
INSERT INTO Region (Region) VALUES ('Metropolitana de Santiago'), ('Valparaíso'), ('Biobío');
GO

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
(1, 'Santiago', '<info><superficie>22.4</superficie><poblacion>200000</poblacion></info>'),
(1, 'Providencia', '<info><superficie>14.4</superficie><poblacion>142000</poblacion></info>'),
(2, 'Valparaíso', '<info><superficie>401.6</superficie><poblacion>296000</poblacion></info>'),
(2, 'Viña del Mar', '<info><superficie>121.6</superficie><poblacion>334000</poblacion></info>'),
(3, 'Concepción', '<info><superficie>221.6</superficie><poblacion>223000</poblacion></info>');
GO
&lt;/details>

Paso 3: Configurar las Aplicaciones
Abre la solución (.sln) con Visual Studio y configura los siguientes archivos:

Proyecto API (GeoChile.Application/appsettings.json):
Asegúrate de que la cadena de conexión y la configuración JWT sean correctas.

JSON

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GeoChileDB;User Id=sa;Password=TuClaveSegura123!;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "EstaEsMiClaveSecretaSuperLargaParaLaPruebaDeValueTech2025",
    "Issuer": "https://localhost:7123", // ⚠️ ¡Poner el puerto de tu API!
    "Audience": "https://localhost:7123" // ⚠️ ¡Poner el puerto de tu API!
  },
  // ...
}
Proyecto Web MVC (GeoChile.Presentation.Web/appsettings.json):
Define la URL base de la API.

JSON

{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7123" // ⚠️ ¡Poner el puerto de tu API!
  },
  // ...
}
Paso 4: Ejecutar la Solución
En Visual Studio, haz clic derecho en la Solución -> Propiedades.
Ve a Proyecto de inicio -> Varios proyectos de inicio.
Establece la Acción de GeoChile.Application y GeoChile.Presentation.Web en Iniciar.
Presiona F5 o el botón "Play". Se abrirán dos ventanas del navegador, una para la API (puedes cerrarla) y otra para la aplicación web.
▶️ Uso de la Aplicación
Navega a la URL de la aplicación GeoChile.Presentation.Web.
Serás redirigido a la página de login. Usa las siguientes credenciales:
Usuario: valuetech
Contraseña: Prueba2025
Una vez autenticado, podrás ver el listado de regiones.
Haz clic en una región para ver sus comunas.
Haz clic en "Editar" en una comuna para modificar su información.
🏗️ Arquitectura del Proyecto
La solución está estructurada siguiendo los principios de Domain-Driven Design (DDD) para una clara separación de responsabilidades.

GeoChile.Domain: El núcleo de la aplicación. Contiene las entidades de negocio (Region, Comuna) y las interfaces de los repositorios. Es agnóstico a la tecnología.
GeoChile.Infrastructure: Implementa el acceso a datos. Contiene las clases de repositorio que llaman a los procedimientos almacenados de SQL Server.
GeoChile.Application: La capa de API Rest. Expone los datos, maneja la autenticación y orquesta las operaciones. Es el punto de entrada ejecutable del backend.
GeoChile.Presentation.Web: La capa de cliente. Es una aplicación MVC que consume la API para mostrar la interfaz de usuario.
