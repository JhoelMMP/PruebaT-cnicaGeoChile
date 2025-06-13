-- Elimina la tabla Comuna si existe (debe ir primero por la llave foránea)
DROP TABLE IF EXISTS dbo.Comuna;
GO

-- Elimina la tabla Region si existe
DROP TABLE IF EXISTS dbo.Region;
GO

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

INSERT INTO Region (Region) VALUES ('Metropolitana de Santiago'), ('Valparaíso');
GO

INSERT INTO Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
(1, 'Santiago', '<info><superficie>22.4</superficie><poblacion>200000</poblacion></info>'),
(1, 'Providencia', '<info><superficie>14.4</superficie><poblacion>142000</poblacion></info>'),
(2, 'Valparaíso', '<info><superficie>401.6</superficie><poblacion>296000</poblacion></info>'),
(2, 'Viña del Mar', '<info><superficie>121.6</superficie><poblacion>334000</poblacion></info>');
GO