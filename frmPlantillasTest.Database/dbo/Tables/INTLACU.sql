CREATE TABLE [dbo].[INTLACU] (
    [ACUINCOD] INT           NOT NULL,
    [ACUDHANY] INT           NOT NULL,
    [ACUDSNOM] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_INTLACU] PRIMARY KEY CLUSTERED ([ACUINCOD] ASC, [ACUDHANY] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del área curricular', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACU', @level2type = N'COLUMN', @level2name = N'ACUINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACU', @level2type = N'COLUMN', @level2name = N'ACUDHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descrpción del área curricular', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACU', @level2type = N'COLUMN', @level2name = N'ACUDSNOM';

