CREATE TABLE [dbo].[SLTLAPO] (
    [APCDLTR] INT      NOT NULL,
    [APDHDAT] DATETIME NULL,
    [APTPAPO] CHAR (1) NOT NULL,
    [APDSDES] TEXT     NOT NULL,
    CONSTRAINT [PK_SLTLAPO] PRIMARY KEY CLUSTERED ([APCDLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SLTLAPO_METLLTR] FOREIGN KEY ([APCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLAPO', @level2type = N'COLUMN', @level2name = N'APCDLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de l''aportació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLAPO', @level2type = N'COLUMN', @level2name = N'APDHDAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Queixa(Q) o Suggeriment(S)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLAPO', @level2type = N'COLUMN', @level2name = N'APTPAPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLAPO', @level2type = N'COLUMN', @level2name = N'APDSDES';

