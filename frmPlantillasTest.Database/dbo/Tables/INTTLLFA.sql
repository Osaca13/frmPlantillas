CREATE TABLE [dbo].[INTTLLFA] (
    [LFAINNIF] VARCHAR (9) NOT NULL,
    [LFACDCNI] VARCHAR (9) NOT NULL,
    [LFACDORD] INT         NOT NULL,
    CONSTRAINT [PK_INTTLLFA] PRIMARY KEY CLUSTERED ([LFAINNIF] ASC, [LFACDCNI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NIF de l’usuari que fa la cerca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTTLLFA', @level2type = N'COLUMN', @level2name = N'LFAINNIF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NIF del usuari que s’ha afegit a favorits.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTTLLFA', @level2type = N'COLUMN', @level2name = N'LFACDCNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ordre dins de la llista de favorits', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTTLLFA', @level2type = N'COLUMN', @level2name = N'LFACDORD';

