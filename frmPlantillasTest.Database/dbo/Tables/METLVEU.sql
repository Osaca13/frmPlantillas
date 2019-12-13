CREATE TABLE [dbo].[METLVEU] (
    [VEUINREL] NUMERIC (18)   NOT NULL,
    [VEUINIDI] INT            NOT NULL,
    [VEUDSNOM] VARCHAR (1024) NOT NULL,
    [VEUCDCON] INT            CONSTRAINT [DF_METLVEU_VEUCDCON] DEFAULT (1) NOT NULL,
    [VEUDTDAT] DATETIME       NOT NULL,
    CONSTRAINT [PK_METLVEU] PRIMARY KEY CLUSTERED ([VEUINREL] ASC, [VEUINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Relació del contingut', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLVEU', @level2type = N'COLUMN', @level2name = N'VEUINREL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLVEU', @level2type = N'COLUMN', @level2name = N'VEUINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del contingut', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLVEU', @level2type = N'COLUMN', @level2name = N'VEUDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de vegades escoltat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLVEU', @level2type = N'COLUMN', @level2name = N'VEUCDCON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de primera vocalització', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLVEU', @level2type = N'COLUMN', @level2name = N'VEUDTDAT';

