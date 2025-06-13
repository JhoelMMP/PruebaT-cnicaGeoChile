# 🚀 Solución Prueba Técnica - GeoChile

Este repositorio contiene la solución completa para la prueba técnica de desarrollador. La aplicación permite gestionar Regiones y Comunas de Chile, siguiendo una arquitectura limpia, segura y escalable.

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red)](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
[![Docker](https://img.shields.io/badge/Docker-gray?logo=docker)](https://www.docker.com/)
[![Arquitectura](https://img.shields.io/badge/Arquitectura-DDD-orange)](https://es.wikipedia.org/wiki/Dise%C3%B1o_guiado_por_el_dominio)

---

## 📋 Tabla de Contenidos
1.  [Tecnologías Utilizadas](#-tecnologías-utilizadas)
2.  [Prerrequisitos](#-prerrequisitos)
3.  [Guía de Instalación y Configuración](#-guía-de-instalación-y-configuración)
    * [Paso 1: Clonar el Repositorio](#paso-1-clonar-el-repositorio)
    * [Paso 2: Levantar la Base de Datos con Docker](#paso-2-levantar-la-base-de-datos-con-docker)
    * [Paso 3: Configurar las Aplicaciones](#paso-3-configurar-las-aplicaciones)
    * [Paso 4: Ejecutar la Solución](#paso-4-ejecutar-la-solución)
4.  [Uso de la Aplicación](#-uso-de-la-aplicación)
5.  [Arquitectura del Proyecto](#-arquitectura-del-proyecto)

---

## ✨ Tecnologías Utilizadas

* **Backend:** ASP.NET Core Web API (.NET 8)
* **Frontend:** ASP.NET Core MVC (.NET 8)
* **Base de Datos:** SQL Server 2022
* **Contenerización:** Docker Desktop
* **Arquitectura:** Diseño Guiado por el Dominio (DDD) en 4 capas.
* **Autenticación:** Basada en Tokens (JWT).
* [cite_start]**Acceso a Datos:** ADO.NET con uso exclusivo de Procedimientos Almacenados. 
* **Logging:** Serilog
* **Estilos:** Bootstrap 5

---

## 🔧 Prerrequisitos

Antes de comenzar, asegúrate de tener instalado el siguiente software:

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) o superior.
* [Docker Desktop](https://www.docker.com/products/docker-desktop/).
* Un IDE como [Visual Studio 2022](https://visualstudio.microsoft.com/es/vs/) o [VS Code](https://code.visualstudio.com/).
* Una herramienta para gestionar la base de datos como [Azure Data Studio](https://azure.microsoft.com/es-es/products/data-studio/) o SSMS.

---

## ⚙️ Guía de Instalación y Configuración

Sigue estos pasos para levantar el entorno completo.

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/JhoelMMP/PruebaT-cnicaGeoChile.git
cd NOMBRE-DE-LA-CARPETA
```
### Paso 2: Levantar la Base de Datos con Docker

1.  Asegúrate de que **Docker Desktop** esté en ejecución en tu máquina.
2.  Abre una terminal (PowerShell, CMD, etc.) y ejecuta el siguiente comando para crear un contenedor de SQL Server:

    ```bash
    docker run --name sql-server-prueba -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Prueba2025!" -p 1433:1433 -d [mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)
    ```
    > 📝NOTA: La clave que se uso fue `Prueba2025`

3.  Conéctate al contenedor usando tu gestor de base de datos preferido con las siguientes credenciales:
    * **Servidor:** `localhost`
    * **Usuario:** `sa`
    * **Contraseña:** La que elegiste en el paso anterior.

4.  Ejecuta los script SQL que se encuentran en el proyecto GeoChile.Application/Scripts para crear la base de datos, las tablas y los procedimientos almacenados necesarios.
   
### Paso 3: Configurar las Aplicaciones
Abre la solución (.sln) con Visual Studio y configura los siguientes archivos:

1.  **Proyecto API (`GeoChile.Application/appsettings.json`):**
    Asegúrate de que la cadena de conexión y la configuración JWT sean correctas.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=PruebaGeoChileDB;User Id=sa;Password=Prueba2025!;TrustServerCertificate=True;"
      },
      "Jwt": {
        "Key": "EstaEsMiClaveSecretaSuperLargaParaLaPruebaDeValueTech2025",
        "Issuer": "https://localhost:7123", // ⚠️ ¡Poner el puerto de tu API!
        "Audience": "https://localhost:7123" // ⚠️ ¡Poner el puerto de tu API!
      },
      // ...
    }
    ```

2.  **Proyecto Web MVC (`GeoChile.Presentation.Web/appsettings.json`):**
    Define la URL base de la API.

    ```json
    {
      "ApiSettings": {
        "BaseUrl": "https://localhost:7123" // ⚠️ ¡Poner el puerto de tu API!
      },
      // ...
    }
    ```
### Paso 4: Ejecutar la Solución
1.  En Visual Studio, haz clic derecho en la **Solución** -> **Propiedades**.
2.  Ve a **Proyecto de inicio** -> **Varios proyectos de inicio**.
3.  Establece la **Acción** de `GeoChile.Application` y `GeoChile.Presentation.Web` en **Iniciar**.
4.  Presiona **F5** o el botón "Play". Se abrirán dos ventanas del navegador, una para la API (puedes cerrarla) y otra para la aplicación web.

---

## ▶️ Uso de la Aplicación
1.  Navega a la URL de la aplicación `GeoChile.Presentation.Web`.
2.  Serás redirigido a la página de login. Usa las siguientes credenciales:
    * **Usuario:** `valuetech`
    * **Contraseña:** `Prueba2025`
3.  Una vez autenticado, podrás ver el listado de regiones.
4.  Haz clic en una región para ver sus comunas.
5.  Haz clic en "Editar" en una comuna para modificar su información.

---

## 🏗️ Arquitectura del Proyecto
La solución está estructurada siguiendo los principios de **Domain-Driven Design (DDD)** para una clara separación de responsabilidades.

* **`GeoChile.Domain`**: El núcleo de la aplicación. Contiene las entidades de negocio (Region, Comuna) y las interfaces de los repositorios. Es agnóstico a la tecnología.
* **`GeoChile.Infrastructure`**: Implementa el acceso a datos. Contiene las clases de repositorio que llaman a los procedimientos almacenados de SQL Server.
* **`GeoChile.Application`**: La capa de API Rest. Expone los datos, maneja la autenticación y orquesta las operaciones. Es el punto de entrada ejecutable del backend.
* **`GeoChile.Presentation.Web`**: La capa de cliente. Es una aplicación MVC que consume la API para mostrar la interfaz de usuario.

---

## 👨‍💻 Autor
**Jhoel Molina**
