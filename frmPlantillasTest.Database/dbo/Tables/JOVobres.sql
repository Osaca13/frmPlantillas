CREATE TABLE [dbo].[JOVobres] (
    [_idRegistro]       INT           NULL,
    [_publicado]        CHAR (1)      NULL,
    [_username]         VARCHAR (20)  NULL,
    [_insercion]        DATETIME      NULL,
    [_modificacion]     DATETIME      NULL,
    [IdObra]            BIGINT        NULL,
    [Titol]             VARCHAR (100) NULL,
    [Usuari_autor]      VARCHAR (25)  NULL,
    [IdModalitat]       INT           NULL,
    [imatgeAmpliada]    VARCHAR (128) NULL,
    [Tecnica]           TEXT          NULL,
    [Descripcio]        TEXT          NULL,
    [validat]           CHAR (1)      NULL,
    [ImatgeReduida]     VARCHAR (128) NULL,
    [arxiu]             VARCHAR (128) NULL,
    [portada_modalitat] CHAR (1)      NULL
);

