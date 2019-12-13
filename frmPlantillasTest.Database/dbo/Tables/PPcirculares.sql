CREATE TABLE [dbo].[PPcirculares] (
    [nro]       SMALLINT  IDENTITY (1, 1) NOT NULL,
    [titulo]    TEXT      NULL,
    [nombre]    CHAR (35) NOT NULL,
    [fechacad]  DATETIME  NOT NULL,
    [fechaalta] DATETIME  NULL,
    [tipo]      CHAR (1)  NOT NULL,
    [apartado]  SMALLINT  NULL
);

