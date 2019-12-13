CREATE TABLE [dbo].[TRTLBUT] (
    [BUTINLTR] INT      NOT NULL,
    [BUTCDIDI] SMALLINT CONSTRAINT [DF_TRTLBUT_BUTCDIDI] DEFAULT (1) NOT NULL,
    [BUTDSTXT] TEXT     NULL,
    CONSTRAINT [PK_TRTLBUT] PRIMARY KEY CLUSTERED ([BUTINLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLBUT_METLLTR] FOREIGN KEY ([BUTINLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de METLLTR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLBUT', @level2type = N'COLUMN', @level2name = N'BUTINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma desitjat de resposta. Per defecte 1 (català)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLBUT', @level2type = N'COLUMN', @level2name = N'BUTCDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text indicant la resposta o el rebuig a la sol.licitud de preinscripció.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLBUT', @level2type = N'COLUMN', @level2name = N'BUTDSTXT';

