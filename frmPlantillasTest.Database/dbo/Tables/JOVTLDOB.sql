CREATE TABLE [dbo].[JOVTLDOB] (
    [DOBINCOD] INT           NOT NULL,
    [DOBINIDI] SMALLINT      NOT NULL,
    [DOBDSTIT] VARCHAR (100) NOT NULL,
    [DOBDSTEC] TEXT          NULL,
    [DOBDSDES] TEXT          NULL,
    [DOBCDMOD] INT           NOT NULL,
    CONSTRAINT [PK_JOVTLDOB] PRIMARY KEY CLUSTERED ([DOBINCOD] ASC, [DOBINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_JOVTLDOB_JOVTLMOD] FOREIGN KEY ([DOBCDMOD], [DOBINIDI]) REFERENCES [dbo].[JOVTLMOD] ([MODINCOD], [MODINIDI]),
    CONSTRAINT [FK_JOVTLDOB_JOVTLOBR] FOREIGN KEY ([DOBINCOD]) REFERENCES [dbo].[JOVTLOBR] ([OBRINCOD]),
    CONSTRAINT [FK_JOVTLDOB_METLIDI] FOREIGN KEY ([DOBINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de l’obra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDOB', @level2type = N'COLUMN', @level2name = N'DOBINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDOB', @level2type = N'COLUMN', @level2name = N'DOBINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Títol de l’obra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDOB', @level2type = N'COLUMN', @level2name = N'DOBDSTIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la tècnica utilitzada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDOB', @level2type = N'COLUMN', @level2name = N'DOBDSTEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l’obra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDOB', @level2type = N'COLUMN', @level2name = N'DOBDSDES';

