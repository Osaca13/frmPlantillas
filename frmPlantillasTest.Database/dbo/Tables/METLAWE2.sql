CREATE TABLE [dbo].[METLAWE2] (
    [AWEINNOD] INT            NOT NULL,
    [AWEINIDI] SMALLINT       NOT NULL,
    [AWEDSTIT] VARCHAR (1024) NOT NULL,
    [AWEDSSER] INT            NOT NULL,
    [AWEDSROT] VARCHAR (1024) NOT NULL,
    [AWEDSUSR] CHAR (10)      NULL,
    [AWEDSTCO] VARCHAR (1024) NULL,
    [AWEDSPLA] VARCHAR (1024) NULL,
    [AWEDSEST] TEXT           NULL,
    [AWEDSHTM] TEXT           CONSTRAINT [DF_METLAWE2_AWEDSHTM] DEFAULT ('<div class=""row border border-secondary p-2""><div class=""col cel border border-secondary p-2"" id=""d0""><span class=""divId"" style=""display:none"">0</span><span class=""divImg""></span><span class=""text"">Cel&middot;la inicial</span><span class=""atributs"" style=""display:none"">0#Cel&middot;la inicial##########################|</span></div></div> ''') NULL,
    [AWEDSLCW] VARCHAR (1024) NULL,
    [AWEDSLC2] VARCHAR (1024) NULL,
    [AWEDSDOC] VARCHAR (1024) CONSTRAINT [DF_METLAWE2_AWEDSDOC] DEFAULT ('') NOT NULL,
    [AWEDSEBO] VARCHAR (300)  CONSTRAINT [DF_METLAWE2_AWEDSEBO] DEFAULT ('') NOT NULL,
    [AWEDSMET] TEXT           CONSTRAINT [DF_METLAWE2_AWEDSMET] DEFAULT ('') NOT NULL,
    [AWEDSPEU] TEXT           CONSTRAINT [DF_METLAWE2_AWEDSPEU] DEFAULT ('') NOT NULL,
    [AWEDSCSP] VARCHAR (1024) NULL,
    [AWEDSCSI] VARCHAR (1024) NULL,
    [AWEDSNOM] VARCHAR (100)  NULL,
    CONSTRAINT [PK_METLAWE2] PRIMARY KEY NONCLUSTERED ([AWEINNOD] ASC, [AWEINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'estils pel body', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE2', @level2type = N'COLUMN', @level2name = N'AWEDSEBO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cadena "META" que compartiran totes les pàgines del web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE2', @level2type = N'COLUMN', @level2name = N'AWEDSMET';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Html que s''afegirà al peu de la pàgina. després del que es pugui trobar a NWEDSPEU i abans del </body>', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE2', @level2type = N'COLUMN', @level2name = N'AWEDSPEU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS que s’han d’incloure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE2', @level2type = N'COLUMN', @level2name = N'AWEDSCSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista separada per “,” dels fitxers CSS per la impressora que s’han d’incloure', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAWE2', @level2type = N'COLUMN', @level2name = N'AWEDSCSI';

