CREATE TABLE [dbo].[CAITLUSU] (
    [USUINUSU] INT           IDENTITY (1, 1) NOT NULL,
    [USUDSADM] CHAR (1)      NOT NULL,
    [USUDSNOM] VARCHAR (50)  NOT NULL,
    [USUDSPER] VARCHAR (50)  NOT NULL,
    [USUDSUSU] VARCHAR (50)  NOT NULL,
    [USUDSPWD] VARCHAR (50)  NOT NULL,
    [USUDSADR] VARCHAR (50)  NOT NULL,
    [USUDSTEL] CHAR (9)      NOT NULL,
    [USUDSFAX] CHAR (9)      NULL,
    [USUDSCOR] VARCHAR (50)  NOT NULL,
    [USUDSPAS] INT           NOT NULL,
    [USUDHLAS] SMALLDATETIME NOT NULL,
    [USUDSCPO] CHAR (5)      NOT NULL,
    [USUDSPOB] VARCHAR (50)  NOT NULL,
    [USUCDCDG] INT           NOT NULL,
    [USUDSOBS] TEXT          NULL,
    [USUDSOBT] VARCHAR (50)  NULL,
    CONSTRAINT [PK_CAITLUSU] PRIMARY KEY CLUSTERED ([USUINUSU] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo autonumero identificativo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUINUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del departamento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Persona de contacto', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificativo del usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Password', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Direccion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSADR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telefono', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSTEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fax', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSFAX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correo Electronico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero de errores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSPAS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo postal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSCPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Poblacion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSPOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo directorio dentro de GAIA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUCDCDG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observaciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSOBS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Para determinar el acceso a la pagina de observatori', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CAITLUSU', @level2type = N'COLUMN', @level2name = N'USUDSOBT';

