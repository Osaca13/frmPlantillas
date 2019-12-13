CREATE TABLE [dbo].[METLAWE] (
    [AWEINNOD]  INT            NOT NULL,
    [AWEINIDI]  SMALLINT       NOT NULL,
    [AWEDSTIT]  VARCHAR (1024) NOT NULL,
    [AWEDSSER]  INT            NOT NULL,
    [AWEDSROT]  VARCHAR (1024) NOT NULL,
    [AWEDSUSR]  CHAR (10)      NULL,
    [AWEDSTHOR] VARCHAR (1024) NULL,
    [AWEDSTVER] VARCHAR (1024) NULL,
    [AWEDSTCO]  VARCHAR (1024) NULL,
    [AWEDSEST]  VARCHAR (1024) NULL,
    [AWEDSATR]  VARCHAR (1024) NULL,
    [AWEDSLCW]  VARCHAR (1024) NULL,
    [AWEDSHOR]  SMALLINT       CONSTRAINT [DF_METLAWE_AWEDSHOR] DEFAULT (100) NOT NULL,
    [AWEDSVER]  SMALLINT       CONSTRAINT [DF_METLAWE_AWEDSVER] DEFAULT (100) NOT NULL,
    [AWEDSDOC]  VARCHAR (1024) CONSTRAINT [DF_METLAWE_AWEDSDOC] DEFAULT ('') NOT NULL,
    [AWEDSEBO]  VARCHAR (300)  CONSTRAINT [DF_METLAWE_AWEDSEBO] DEFAULT ('') NOT NULL,
    [AWEDSMET]  TEXT           CONSTRAINT [DF_METLAWE_AWEDSMET] DEFAULT ('') NOT NULL,
    [AWEDSPEU]  TEXT           CONSTRAINT [DF_METLAWE_AWEDSPEU] DEFAULT ('') NOT NULL,
    [AWEDSCSP]  VARCHAR (1024) NULL,
    [AWEDSCSI]  VARCHAR (1024) NULL,
    [AWEDSNOM]  VARCHAR (100)  NULL,
    CONSTRAINT [PK_METLAWE] PRIMARY KEY NONCLUSTERED ([AWEINNOD] ASC, [AWEINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLAWE_7_1685633098__K1_12]
    ON [dbo].[METLAWE]([AWEINNOD] ASC)
    INCLUDE([AWEDSLCW]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'estils pel body', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE', @level2type = N'COLUMN', @level2name = N'AWEDSEBO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cadena "META" que compartiran totes les pàgines del web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE', @level2type = N'COLUMN', @level2name = N'AWEDSMET';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Html que s''afegirà al peu de la pàgina. després del que es pugui trobar a NWEDSPEU i abans del </body>', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE', @level2type = N'COLUMN', @level2name = N'AWEDSPEU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS que s’han d’incloure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE', @level2type = N'COLUMN', @level2name = N'AWEDSCSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS per la impressora que s’han d’incloure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE', @level2type = N'COLUMN', @level2name = N'AWEDSCSI';

