CREATE TABLE [dbo].[CCITLSRE] (
    [SREINEXP] INT            NOT NULL,
    [SRESTCOR] VARCHAR (250)  NOT NULL,
    [SRESTADR] VARCHAR (1000) NULL,
    [SRESTCPR] CHAR (10)      NOT NULL,
    [SRESTCPA] CHAR (10)      NOT NULL,
    [SRESTNRE] CHAR (10)      NULL,
    [SREDTREG] DATETIME       NULL,
    CONSTRAINT [PK_CCITLSRE] PRIMARY KEY CLUSTERED ([SREINEXP] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CCITLSRE_METLLTR] FOREIGN KEY ([SREINEXP]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CCITLSRE]
    ON [dbo].[CCITLSRE]([SRESTCPA] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi identificatiu de l''expedient', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SREINEXP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu electrónic del sol·licitant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SRESTCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça del sol·licitant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SRESTADR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau per al primer accés', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SRESTCPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau de la página del primer acés, la que s''envia per correu', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SRESTCPA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número en el registre d''entrada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSRE', @level2type = N'COLUMN', @level2name = N'SRESTNRE';

