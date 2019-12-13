CREATE TABLE [dbo].[INTLAMB] (
    [AMBINAMB] INT       NOT NULL,
    [AMBDHANY] INT       NOT NULL,
    [AMBDSNOM] CHAR (70) NOT NULL,
    CONSTRAINT [PK_INTLAMB] PRIMARY KEY CLUSTERED ([AMBINAMB] ASC, [AMBDHANY] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del ámbito', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAMB', @level2type = N'COLUMN', @level2name = N'AMBINAMB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAMB', @level2type = N'COLUMN', @level2name = N'AMBDHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del ámbito', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAMB', @level2type = N'COLUMN', @level2name = N'AMBDSNOM';

