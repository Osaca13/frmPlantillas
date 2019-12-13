CREATE TABLE [dbo].[INTLESC] (
    [ESCINESC] INT            IDENTITY (1, 1) NOT NULL,
    [ESCDSNOM] CHAR (50)      NOT NULL,
    [ESCDSPER] CHAR (50)      NOT NULL,
    [ESCDSUSU] CHAR (8)       NULL,
    [ESCDSPWD] VARCHAR (5000) NULL,
    [ESCDSUSF] CHAR (8)       CONSTRAINT [DF_INTLESC_ESCDSUSF] DEFAULT ('') NULL,
    [ESCDSPWF] VARCHAR (5000) CONSTRAINT [DF_INTLESC_ESCDSPWF] DEFAULT ('') NULL,
    [ESCDSCOR] CHAR (50)      NOT NULL,
    [ESCDSPAS] INT            NOT NULL,
    [ESCTPTIP] CHAR (1)       NOT NULL,
    [ESCINCDG] NUMERIC (18)   CONSTRAINT [DF_INTLESC_ESCINCDG] DEFAULT ((0)) NOT NULL,
    [ESCINPWD] INT            CONSTRAINT [DF_INTLESC_ESCINPWD] DEFAULT ((0)) NULL,
    [ESCINPWF] INT            CONSTRAINT [DF_INTLESC_ESCINPWF] DEFAULT ((0)) NULL,
    [ESCDSPE2] CHAR (50)      NULL,
    [ESCDSTEL] CHAR (9)       NULL,
    [ESCDSTE2] CHAR (9)       NULL,
    [ESCDSFAX] CHAR (9)       NULL,
    [ESCDSMUN] VARCHAR (50)   NULL,
    [ESCDSCOP] CHAR (5)       NULL,
    [ESCDSDIR] VARCHAR (50)   NULL,
    [ESCSWBRE] CHAR (1)       NULL,
    CONSTRAINT [PK_INTLESC] PRIMARY KEY CLUSTERED ([ESCINESC] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo de escuela', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCINESC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la escuela', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Persona de contacto con la escuela', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador único de usuario de la escuela', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contraseña para acceder al INTRO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador de usuario del forum.Ahora identificador de la escuela', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSUSF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contraseña para acceder a los Forums', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSPWF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correo electrónico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero de errores en el password. Si = 3 usuario bloqueado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSPAS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de escuela. ''P'' Publica.''C'' Concertada. ''R'' Privada.''A'' Administraciones. ''E'' Entidades. ''*'' Prueba', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCTPTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo directorio dentro de GAIA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCINCDG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contraseña del Foro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCINPWF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FAX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSFAX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Municipio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSMUN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código postal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSCOP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirección', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLESC', @level2type = N'COLUMN', @level2name = N'ESCDSDIR';

