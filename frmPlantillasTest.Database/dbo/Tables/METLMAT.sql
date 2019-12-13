CREATE TABLE [dbo].[METLMAT] (
    [MATINNOD] INT      NOT NULL,
    [MATINORG] INT      NOT NULL,
    [MATINIDI] INT      NOT NULL,
    [MATDTTIM] DATETIME NOT NULL,
    [MATDTAVI] DATETIME NULL,
    CONSTRAINT [PK_METLMAT_1] PRIMARY KEY CLUSTERED ([MATINNOD] ASC, [MATINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de node del contingut pendent de traduir', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAT', @level2type = N'COLUMN', @level2name = N'MATINNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de persona que ha modificat el contingut original', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAT', @level2type = N'COLUMN', @level2name = N'MATINORG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Idioma pendent de traducció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAT', @level2type = N'COLUMN', @level2name = N'MATINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data (dia i hora) de modificació del contingut original', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAT', @level2type = N'COLUMN', @level2name = N'MATDTTIM';

