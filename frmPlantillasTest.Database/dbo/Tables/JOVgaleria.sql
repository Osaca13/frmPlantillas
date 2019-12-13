CREATE TABLE [dbo].[JOVgaleria] (
    [_idRegistro]    INT           NULL,
    [_publicado]     CHAR (1)      NULL,
    [_username]      VARCHAR (20)  NULL,
    [_insercion]     DATETIME      NULL,
    [_modificacion]  DATETIME      NULL,
    [idfoto]         INT           NULL,
    [foto]           VARCHAR (128) NULL,
    [comentarifoto]  VARCHAR (255) NULL,
    [autor]          VARCHAR (150) NULL,
    [destacat]       CHAR (1)      NULL,
    [ampliacio_foto] VARCHAR (128) NULL,
    [galeria]        VARCHAR (255) NULL,
    [idGaleria]      INT           NULL
);

