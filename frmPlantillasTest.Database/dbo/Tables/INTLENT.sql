CREATE TABLE [dbo].[INTLENT] (
    [ENTINENT] INT            IDENTITY (1, 1) NOT NULL,
    [ENTDHANY] INT            NOT NULL,
    [ENTDSNOM] CHAR (100)     NOT NULL,
    [ENTDSDEP] CHAR (100)     NULL,
    [ENTDSPER] CHAR (50)      NOT NULL,
    [ENTDSUSU] CHAR (8)       NOT NULL,
    [ENTDSPWD] VARCHAR (5000) NOT NULL,
    [ENTDSADR] CHAR (50)      CONSTRAINT [DF_INTLENT_ENTDSADR] DEFAULT ('') NOT NULL,
    [ENTDSTEL] CHAR (9)       CONSTRAINT [DF_INTLENT_ENTDSTEL] DEFAULT ('') NOT NULL,
    [ENTDSFAX] CHAR (9)       CONSTRAINT [DF_INTLENT_ENTDSFAX] DEFAULT ('') NOT NULL,
    [ENTDSCOR] CHAR (55)      NOT NULL,
    [ENTDSPAS] INT            NOT NULL,
    [ENTDSCPO] CHAR (5)       CONSTRAINT [DF_INTLENT_ENTDSCPO] DEFAULT ('') NOT NULL,
    [ENTDSPOB] CHAR (50)      CONSTRAINT [DF_INTLENT_ENTDSPOB] DEFAULT ('') NOT NULL,
    [ENTSWPLE] INT            CONSTRAINT [DF_INTLENT_ENTSWPLE] DEFAULT ((0)) NOT NULL,
    [ENTDSPCI] CHAR (50)      CONSTRAINT [DF_INTLENT_ENTDSPCI] DEFAULT ('') NOT NULL,
    [ENTDSTCI] CHAR (9)       CONSTRAINT [DF_INTLENT_ENTDSTCI] DEFAULT ('') NOT NULL,
    [ENTDSCCI] CHAR (150)     CONSTRAINT [DF_INTLENT_ENTDSCCI] DEFAULT ('') NOT NULL,
    [ENTDSWWW] CHAR (80)      CONSTRAINT [DF_INTLENT_ENTDSWWW] DEFAULT ('') NOT NULL,
    [ENTINCDG] INT            CONSTRAINT [DF_INTLENT_ENTINCDG] DEFAULT ((0)) NOT NULL,
    [ENTDSOBS] TEXT           NULL,
    [ENTINPWD] INT            CONSTRAINT [DF_INTLENT_ENTINFPW] DEFAULT ((0)) NULL,
    [ENTDSPC2] CHAR (50)      NULL,
    [ENTDSTC2] CHAR (9)       NULL,
    [ENTDSCC2] CHAR (55)      NULL,
    CONSTRAINT [PK_INTLENT] PRIMARY KEY CLUSTERED ([ENTINENT] ASC, [ENTDHANY] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo entidad proveedora de actividades', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTINENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año en el que la entidad tiene actividades ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del proveedor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Persona de contacto de la entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador único de la persona  de la entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clave para acceder al INTRO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirección de la entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSADR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Teléfono', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSTEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FAX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSFAX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correo electrónico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de errores en el password.Si = 3 usuario bloqueado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSPAS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Postal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSCPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Población', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSPOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de plazas por entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTSWPLE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Persona de contacto con el INTRO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSPCI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Teléfono de contacto con el INTRO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSTCI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correo electrónico de la persona del INTRO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSCCI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WWW de la entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTDSWWW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo directorio dentro de GAIA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLENT', @level2type = N'COLUMN', @level2name = N'ENTINCDG';

