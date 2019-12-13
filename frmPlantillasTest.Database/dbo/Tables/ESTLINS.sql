CREATE TABLE [dbo].[ESTLINS] (
    [INSININS] INT           IDENTITY (1, 1) NOT NULL,
    [INSDSNOM] VARCHAR (150) NOT NULL,
    [INSDSUSU] VARCHAR (10)  NOT NULL,
    [INSDSPWD] VARCHAR (10)  NOT NULL,
    [INSWNINT] SMALLINT      CONSTRAINT [DF_ESTLINS_INSININT] DEFAULT ((0)) NOT NULL,
    [INSDHACC] DATETIME      NULL,
    CONSTRAINT [PK_ESTLINS] PRIMARY KEY CLUSTERED ([INSININS] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Instal·lació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLINS', @level2type = N'COLUMN', @level2name = N'INSININS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom instal·lació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLINS', @level2type = N'COLUMN', @level2name = N'INSDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLINS', @level2type = N'COLUMN', @level2name = N'INSDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLINS', @level2type = N'COLUMN', @level2name = N'INSDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Intents d''accés', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLINS', @level2type = N'COLUMN', @level2name = N'INSWNINT';

