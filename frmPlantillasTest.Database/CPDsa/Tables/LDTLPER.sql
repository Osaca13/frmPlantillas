CREATE TABLE [CPDsa].[LDTLPER] (
    [PERINPER] INT           IDENTITY (1, 1) NOT NULL,
    [PERDSMAI] VARCHAR (100) NOT NULL,
    [PERDSNOM] VARCHAR (200) NOT NULL,
    [PERDSCG1] VARCHAR (25)  NOT NULL,
    [PERDSCG2] VARCHAR (25)  NOT NULL,
    [PERSWMSC] CHAR (1)      NULL,
    [PERDTNAI] DATE          NULL,
    [PERINTEL] VARCHAR (11)  NULL,
    [PERINADR] VARCHAR (100) NULL,
    [PERINPOB] VARCHAR (30)  NULL,
    [PERINCOP] VARCHAR (5)   NULL,
    CONSTRAINT [PK_LDTLPER] PRIMARY KEY CLUSTERED ([PERINPER] ASC),
    CONSTRAINT [IX_LDTLPER] UNIQUE NONCLUSTERED ([PERDSMAI] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Persona', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERINPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERDSMAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom1', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERDSCG1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom2', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERDSCG2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sexe masculí?', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLPER', @level2type = N'COLUMN', @level2name = N'PERSWMSC';

