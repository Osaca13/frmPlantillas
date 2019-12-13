CREATE TABLE [dbo].[JOVautors] (
    [_idRegistro]    INT           NULL,
    [_publicado]     CHAR (1)      NULL,
    [_username]      VARCHAR (20)  NULL,
    [_insercion]     DATETIME      NULL,
    [_modificacion]  DATETIME      NULL,
    [nom_usuari]     VARCHAR (10)  NULL,
    [Nom_Cognoms]    VARCHAR (150) NULL,
    [Foto]           VARCHAR (128) NULL,
    [Data_Naixement] VARCHAR (50)  NULL,
    [Adresa]         VARCHAR (150) NULL,
    [Telefon]        VARCHAR (15)  NULL,
    [Fax]            VARCHAR (15)  NULL,
    [email]          VARCHAR (50)  NULL,
    [resenya]        TEXT          NULL,
    [llocNaixement]  VARCHAR (75)  NULL,
    [id_autor]       INT           NULL,
    [contrasenya]    VARCHAR (15)  NULL,
    [grup]           VARCHAR (15)  NULL,
    [validat]        CHAR (1)      NULL,
    [idIdioma]       INT           NULL
);

