CREATE TABLE [dbo].[METLWEB2] (
    [WEBINNOD] NUMERIC (18)   NOT NULL,
    [WEBINIDI] SMALLINT       NOT NULL,
    [WEBDSTIT] VARCHAR (1024) NOT NULL,
    [WEBTPBUS] CHAR (1)       CONSTRAINT [DF_METLWEB2_WEBTPBUS] DEFAULT ('N') NOT NULL,
    [WEBDTANY] DATETIME       NOT NULL,
    [WEBDTCAD] DATETIME       NOT NULL,
    [WEBDTPUB] DATETIME       NOT NULL,
    [WEBDSUSR] CHAR (10)      NOT NULL,
    [WEBDSTCO] VARCHAR (1024) NOT NULL,
    [WEBDSPLA] VARCHAR (1024) NOT NULL,
    [WEBDSEST] TEXT           NOT NULL,
    [WEBDSHTM] TEXT           NOT NULL,
    [WEBDSLCW] VARCHAR (1024) NOT NULL,
    [WEBDSFIT] VARCHAR (1024) NOT NULL,
    [WEBDSURL] VARCHAR (1024) NOT NULL,
    [WEBDSCSS] VARCHAR (2024) NULL,
    [WEBTPHER] CHAR (1)       CONSTRAINT [DF_METLWEB2_WEBTPHER] DEFAULT ('S') NOT NULL,
    [WEBCDRAL] INT            CONSTRAINT [DF_METLWEB2_WEBCDRAL] DEFAULT ((0)) NOT NULL,
    [WEBDSNST] VARCHAR (1024) CONSTRAINT [DF_METLWEB2_WEBDSNST] DEFAULT ('') NOT NULL,
    [WEBDSIMP] VARCHAR (1024) CONSTRAINT [DF_METLWEB2_WEBDSIMP] DEFAULT ((1)) NOT NULL,
    [WEBWNMTH] INT            CONSTRAINT [DF_METLWEB2_WEBWNMTH] DEFAULT ((730)) NOT NULL,
    [WEBDSCND] VARCHAR (1024) CONSTRAINT [DF_METLWEB2_WEBDSCND] DEFAULT ((1)) NOT NULL,
    [WEBSWFRM] CHAR (1)       CONSTRAINT [DF_METLWEB2_WEBSWFRM] DEFAULT ('N') NOT NULL,
    [WEBSWEML] CHAR (1)       CONSTRAINT [DF_METLWEB2_WEBSWEML] DEFAULT ('N') NOT NULL,
    [WEBDSEBO] VARCHAR (200)  CONSTRAINT [DF_METLWEB2_WEBDSEBO] DEFAULT ('') NOT NULL,
    [WEBDSDES] VARCHAR (300)  CONSTRAINT [DF_METLWEB2_WEBDSDES] DEFAULT ('') NOT NULL,
    [WEBDSPCL] VARCHAR (300)  CONSTRAINT [DF_METLWEB2_WEBDSPCL] DEFAULT ('') NOT NULL,
    [WEBSWSSL] CHAR (1)       CONSTRAINT [DF_METLWEB2_WEBSWSSL] DEFAULT ('N') NOT NULL,
    [WEBDSLC2] VARCHAR (1024) NULL,
    CONSTRAINT [PK_METLWEB2] PRIMARY KEY NONCLUSTERED ([WEBINNOD] ASC, [WEBINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLWEB2_METLIDI] FOREIGN KEY ([WEBINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLWEB2_METLNOD] FOREIGN KEY ([WEBINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Incloure-la al cercador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBTPBUS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus contingut de las cellas separats per ,', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSTCO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estructura', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código a representar en frmPlantilla y frmEstructura, con nombres de las celdas...', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSHTM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del fichero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSFIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'url', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSURL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Herencia dels nodes web i arbre web superiors', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBTPHER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de valors que indiiquen si es pot imprimir la cel·la o no', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSIMP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mida horitzontal maxima (px) nomes si no hereta propietats', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBWNMTH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de valors que indiiquen si es mostrarà el missatge de contingut no disponible', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSCND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és un formulari ?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBSWFRM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és un correu electrónic? (només per mailing)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBSWEML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estils pel body. Només si no hereta propietats', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSEBO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la pàgina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Paraules clau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBDSPCL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és una pàgina segura?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB2', @level2type = N'COLUMN', @level2name = N'WEBSWSSL';

