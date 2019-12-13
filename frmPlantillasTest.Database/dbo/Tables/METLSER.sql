CREATE TABLE [dbo].[METLSER] (
    [SERINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [SERDSDIR] CHAR (30)    NOT NULL,
    [SERDSCAM] TEXT         NOT NULL,
    [SERDSUSR] CHAR (30)    NOT NULL,
    [SERDSPWD] CHAR (30)    NOT NULL,
    [SERDSPRT] INT          NULL,
    [SERDSURL] VARCHAR (50) CONSTRAINT [DF_METLSER_SERDSURL] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_METLSER] PRIMARY KEY CLUSTERED ([SERINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del servidor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça del servidor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERDSDIR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Directori arrel on hi ha les pàgines', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERDSCAM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari per fer login', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERDSUSR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contrasenya encriptada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PORT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLSER', @level2type = N'COLUMN', @level2name = N'SERDSPRT';

