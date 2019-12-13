CREATE TABLE [dbo].[SLTLDTB] (
    [DTBINLTR] INT           NOT NULL,
    [DTBDSSS]  CHAR (60)     NOT NULL,
    [DTBWNTS]  CHAR (60)     NOT NULL,
    [DTBDSLLT] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SLTLDTB] PRIMARY KEY CLUSTERED ([DTBINLTR] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLDTB', @level2type = N'COLUMN', @level2name = N'DTBINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la Seguritat Social', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLDTB', @level2type = N'COLUMN', @level2name = N'DTBDSSS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la targeta sanitària', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLDTB', @level2type = N'COLUMN', @level2name = N'DTBWNTS';

