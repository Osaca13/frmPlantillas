CREATE TABLE [dbo].[JOVTLOBR] (
    [OBRINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [OBRCDAUT] INT           NOT NULL,
    [OBRDSFIT] VARCHAR (200) NOT NULL,
    [OBRCDPOR] CHAR (1)      CONSTRAINT [DF_JOVTLOBR_OBRCDPOR] DEFAULT ('N') NOT NULL,
    CONSTRAINT [PK_JOVTLOBR] PRIMARY KEY CLUSTERED ([OBRINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_JOVTLOBR_JOVTLAUT] FOREIGN KEY ([OBRCDAUT]) REFERENCES [dbo].[JOVTLAUT] ([AUTINCOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de l’obra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLOBR', @level2type = N'COLUMN', @level2name = N'OBRINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'És imatge de portada?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLOBR', @level2type = N'COLUMN', @level2name = N'OBRCDPOR';

