CREATE TABLE [dbo].[METLNWE] (
    [NWEINNOD]  INT            NOT NULL,
    [NWEINIDI]  SMALLINT       NOT NULL,
    [NWEDSTIT]  VARCHAR (1024) CONSTRAINT [DF_METLNEW_NEWDSTIT] DEFAULT ('') NOT NULL,
    [NWEDSCAR]  VARCHAR (1024) CONSTRAINT [DF_METLNEW_NEWDSCAR] DEFAULT ('') NOT NULL,
    [NWEDSUSR]  CHAR (10)      NULL,
    [NWEDSTHOR] VARCHAR (1024) NULL,
    [NWEDSTVER] VARCHAR (1024) NULL,
    [NWEDSTCO]  VARCHAR (1024) NULL,
    [NWEDSEST]  VARCHAR (1024) CONSTRAINT [DF_METLNWE_NWEDSEST] DEFAULT ('') NOT NULL,
    [NWEDSATR]  VARCHAR (1024) NULL,
    [NWEDSLCW]  VARCHAR (1024) NULL,
    [NWEDSCSS]  VARCHAR (2024) NULL,
    [NWEDSPLA]  VARCHAR (1024) CONSTRAINT [DF_METLNWE_NWEDSPLA] DEFAULT ('') NOT NULL,
    [NWEDSEBO]  VARCHAR (200)  CONSTRAINT [DF_METLNWE_NWEDSEBO] DEFAULT ('') NOT NULL,
    [NWEDSMET]  TEXT           CONSTRAINT [DF_METLNWE_NWEDSMET] DEFAULT ('') NOT NULL,
    [NWEDSPEU]  TEXT           CONSTRAINT [DF_METLNWE_NWEDSPEU] DEFAULT ('') NOT NULL,
    [NWEDSCSP]  VARCHAR (1024) CONSTRAINT [DF_METLNWE_NWEDSCSP] DEFAULT ('') NOT NULL,
    [NWEDSCSI]  VARCHAR (1024) CONSTRAINT [DF_METLNWE_NWEDSCSI] DEFAULT ('') NOT NULL,
    [NWEDTPUB]  DATETIME       CONSTRAINT [DF_METLNWE_NWEDTPUB] DEFAULT (1 / 1 / 1900) NOT NULL,
    [NWEDTCAD]  DATETIME       CONSTRAINT [DF_METLNWE_NWEDTCAD] DEFAULT (1 / 1 / 1900) NOT NULL,
    CONSTRAINT [PK_METLNEW] PRIMARY KEY NONCLUSTERED ([NWEINNOD] ASC, [NWEINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLNWE_7_317296240__K1_11_13]
    ON [dbo].[METLNWE]([NWEINNOD] ASC)
    INCLUDE([NWEDSLCW], [NWEDSPLA]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista d’estils CSS per cel•la', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSCSS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de plantilles aplicables per cel•la', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSPLA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'estils pel body', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSEBO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tags Meta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSMET';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Html per afegir al Peu. S''afegirà abans del html que es troba a METLAWE.AWEDSPEU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSPEU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS per la pantalla  que s’han d’incloure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSCSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS per la impresora que s’han d’incloure ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNWE', @level2type = N'COLUMN', @level2name = N'NWEDSCSI';

