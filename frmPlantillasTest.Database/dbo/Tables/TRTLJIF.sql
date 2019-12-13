CREATE TABLE [dbo].[TRTLJIF] (
    [JIFCDLTR] INT      NOT NULL,
    [JIFSWVA1] CHAR (1) CONSTRAINT [DF_TRTLJIF_MAPSWVA1] DEFAULT ('N') NULL,
    [JIFSWVA2] CHAR (1) CONSTRAINT [DF_TRTLJIF_MAPSWVA2] DEFAULT ('N') NULL,
    [JIFSWVA3] CHAR (1) CONSTRAINT [DF_TRTLJIF_MAPSWVA3] DEFAULT ('N') NULL,
    CONSTRAINT [PK_TRTLJIF] PRIMARY KEY CLUSTERED ([JIFCDLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLJIF_METLLTR] FOREIGN KEY ([JIFCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de METLLTR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLJIF', @level2type = N'COLUMN', @level2name = N'JIFCDLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Camp variable 1 (). Valor “S” / “N”. Per defecte “N”', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLJIF', @level2type = N'COLUMN', @level2name = N'JIFSWVA1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Camp variable 1 (). Valor “S” / “N”. Per defecte “N”', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLJIF', @level2type = N'COLUMN', @level2name = N'JIFSWVA2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Camp variable 1 (). Valor “S” / “N”. Per defecte “N”', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLJIF', @level2type = N'COLUMN', @level2name = N'JIFSWVA3';

