CREATE TABLE [dbo].[circulares] (
    [nro]       SMALLINT   IDENTITY (1, 1) NOT NULL,
    [titulo]    TEXT       NULL,
    [nombre]    CHAR (100) NOT NULL,
    [fechacad]  DATETIME   NOT NULL,
    [fechaalta] DATETIME   NULL,
    [tipo]      CHAR (1)   NOT NULL,
    [apartado]  SMALLINT   NULL
);

