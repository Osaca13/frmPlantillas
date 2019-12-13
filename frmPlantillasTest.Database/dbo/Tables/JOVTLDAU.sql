CREATE TABLE [dbo].[JOVTLDAU] (
    [DAUINCOD] INT      NOT NULL,
    [DAUINIDI] SMALLINT NOT NULL,
    [DAUDSRES] TEXT     NULL,
    CONSTRAINT [PK_JOVTLADE] PRIMARY KEY CLUSTERED ([DAUINCOD] ASC, [DAUINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_JOVTLDAU_JOVTLAUT] FOREIGN KEY ([DAUINCOD]) REFERENCES [dbo].[JOVTLAUT] ([AUTINCOD]) ON DELETE CASCADE,
    CONSTRAINT [FK_JOVTLDAU_METLIDI] FOREIGN KEY ([DAUINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d’autor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDAU', @level2type = N'COLUMN', @level2name = N'DAUINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma del text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDAU', @level2type = N'COLUMN', @level2name = N'DAUINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text lliure per descriure l’obra de l’autor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLDAU', @level2type = N'COLUMN', @level2name = N'DAUDSRES';

