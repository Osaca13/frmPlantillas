CREATE TABLE [dbo].[METLCEL] (
    [CELINLCW] INT           NOT NULL,
    [CELINIDI] INT           NOT NULL,
    [CELINREI] INT           NOT NULL,
    [CELINREL] INT           NOT NULL,
    [CELCDEST] INT           NOT NULL,
    [CELCDUSU] INT           NOT NULL,
    [CELCDNOD] INT           NOT NULL,
    [CELCDPLT] INT           NOT NULL,
    [CELDTFEC] DATETIME      NOT NULL,
    [CELDTCAD] DATETIME      NOT NULL,
    [CELDSCSS] TEXT          NOT NULL,
    [CELDSEXE] TEXT          NOT NULL,
    [CELINPAR] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_METLCEL_1] PRIMARY KEY CLUSTERED ([CELINPAR] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLCEL_7_1970926193__K1_13]
    ON [dbo].[METLCEL]([CELINLCW] ASC)
    INCLUDE([CELINPAR]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLCEL_7_1970926193__K7]
    ON [dbo].[METLCEL]([CELCDNOD] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLCEL_7_1970926193__K4_K7_K5]
    ON [dbo].[METLCEL]([CELINREL] ASC, [CELCDNOD] ASC, [CELCDEST] ASC);


GO
CREATE NONCLUSTERED INDEX [METLCEL_CELCDPLT]
    ON [dbo].[METLCEL]([CELCDPLT] ASC)
    INCLUDE([CELINPAR]);


GO
CREATE STATISTICS [_dta_stat_1970926193_7_4_5]
    ON [dbo].[METLCEL]([CELCDNOD], [CELINREL], [CELCDEST]);


GO
CREATE STATISTICS [_dta_stat_1970926193_5_7]
    ON [dbo].[METLCEL]([CELCDEST], [CELCDNOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'identificador de llibreria de codi', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELINLCW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de relació incial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELINREI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de relació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELINREL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi d''estructura', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELCDEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'usuari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELCDUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELCDNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de plantilla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELCDPLT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data de creació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELDTFEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data de caducitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELDTCAD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'css associat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELDSCSS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de llibreria o cel·la generada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELDSEXE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'paràmetres de crida a la llibreria', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCEL', @level2type = N'COLUMN', @level2name = N'CELINPAR';

