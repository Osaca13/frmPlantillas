CREATE TABLE [dbo].[FORTLARE] (
    [AREINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [AREDSNOM] VARCHAR (100) NOT NULL,
    [ARESWACT] CHAR (1)      CONSTRAINT [DF_FORTLARE_ARESWACT] DEFAULT ('S') NOT NULL,
    CONSTRAINT [PK_FORTLARE] PRIMARY KEY CLUSTERED ([AREINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_FORTLARE]
    ON [dbo].[FORTLARE]([AREINCOD] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Formació. Codi d’àrea', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLARE', @level2type = N'COLUMN', @level2name = N'AREINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de l’àrea', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLARE', @level2type = N'COLUMN', @level2name = N'AREDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Àrea activa?. Per defecte S.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLARE', @level2type = N'COLUMN', @level2name = N'ARESWACT';

