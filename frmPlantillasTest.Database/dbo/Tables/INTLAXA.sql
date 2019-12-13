CREATE TABLE [dbo].[INTLAXA] (
    [AXAINACT] INT NOT NULL,
    [AXADHANY] INT NOT NULL,
    [AXAINAMB] INT NOT NULL,
    CONSTRAINT [PK_INTLAXA] PRIMARY KEY CLUSTERED ([AXAINACT] ASC, [AXADHANY] ASC, [AXAINAMB] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de la actividad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAXA', @level2type = N'COLUMN', @level2name = N'AXAINACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAXA', @level2type = N'COLUMN', @level2name = N'AXADHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del ámbito', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLAXA', @level2type = N'COLUMN', @level2name = N'AXAINAMB';

