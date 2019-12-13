CREATE TABLE [dbo].[LHTLPRO] (
    [PROCDPRO] INT           NOT NULL,
    [PROINIDI] SMALLINT      NOT NULL,
    [PRODSNOM] VARCHAR (100) NOT NULL,
    [PRODSDES] TEXT          NULL,
    [PRODTINI] DATETIME      NOT NULL,
    [PRODTFIN] DATETIME      NOT NULL,
    [PROWNPLA] INT           NOT NULL,
    [PRODSTXT] TEXT          NULL,
    [PROCDNOD] NUMERIC (18)  NULL,
    CONSTRAINT [PK_LHTLPRO] PRIMARY KEY CLUSTERED ([PROCDPRO] ASC, [PROINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código Promoción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PROCDPRO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PROINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre promoción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PRODSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PRODSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de Inicio ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PRODTINI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha Fin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PRODTFIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de places x persona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PROWNPLA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Texto legal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PRODSTXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Node Projecte GAIA ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLPRO', @level2type = N'COLUMN', @level2name = N'PROCDNOD';

