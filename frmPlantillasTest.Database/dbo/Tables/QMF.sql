CREATE TABLE [dbo].[QMF] (
    [nro]       SMALLINT IDENTITY (1, 1) NOT NULL,
    [consulta]  TEXT     NOT NULL,
    [respuesta] TEXT     NULL,
    [fechacad]  DATETIME NOT NULL
);

