-- Obtener todas las regiones
CREATE PROCEDURE sp_GetRegiones
AS
BEGIN
    SELECT IdRegion, Region FROM Region;
END
GO

-- Obtener comunas por regi�n
CREATE PROCEDURE sp_GetComunasPorRegion
    @IdRegion INT
AS
BEGIN
    SELECT IdComuna, IdRegion, Comuna, InformacionAdicional FROM Comuna WHERE IdRegion = @IdRegion;
END
GO

-- Actualizar/Insertar Comuna con MERGE
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
            IdRegion = Source.IdRegion
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (IdRegion, Comuna, InformacionAdicional)
        VALUES (Source.IdRegion, Source.Comuna, Source.InformacionAdicional);
END
GO

-- Scripts para obtener datos de una sola entidad
CREATE PROCEDURE sp_GetRegionPorId @IdRegion INT
AS BEGIN SELECT IdRegion, Region FROM Region WHERE IdRegion = @IdRegion; END
GO

CREATE PROCEDURE sp_GetComunaPorIdRegionId @IdRegion INT, @IdComuna INT
AS BEGIN SELECT IdComuna, IdRegion, Comuna, InformacionAdicional FROM Comuna WHERE IdComuna = @IdComuna AND IdRegion = @IdRegion; END
GO