CREATE TABLE [dbo].[METLWEB] (
    [WEBINNOD]  NUMERIC (18)   NOT NULL,
    [WEBINIDI]  SMALLINT       NOT NULL,
    [WEBDSTIT]  VARCHAR (1024) NOT NULL,
    [WEBTPBUS]  CHAR (1)       CONSTRAINT [DF_METLWEB_WEBTPBUS] DEFAULT ('N') NOT NULL,
    [WEBDTANY]  DATETIME       NOT NULL,
    [WEBDTCAD]  DATETIME       NOT NULL,
    [WEBDTPUB]  DATETIME       NOT NULL,
    [WEBDSUSR]  CHAR (10)      NOT NULL,
    [WEBDSTHOR] VARCHAR (1024) NOT NULL,
    [WEBDSTVER] VARCHAR (1024) NOT NULL,
    [WEBDSTCO]  VARCHAR (1024) NOT NULL,
    [WEBDSPLA]  VARCHAR (1024) NOT NULL,
    [WEBDSEST]  VARCHAR (1024) NOT NULL,
    [WEBDSATR]  VARCHAR (1024) NOT NULL,
    [WEBDSLCW]  VARCHAR (1024) NOT NULL,
    [WEBDSFIT]  VARCHAR (1024) NOT NULL,
    [WEBDSURL]  VARCHAR (1024) NOT NULL,
    [WEBDSCSS]  VARCHAR (2024) NULL,
    [WEBDSCIP]  INT            NOT NULL,
    [WEBDSCIC]  INT            NOT NULL,
    [WEBTPHER]  CHAR (1)       CONSTRAINT [DF_METLWEB_WEBTPHER] DEFAULT ('S') NOT NULL,
    [WEBCDRAL]  INT            CONSTRAINT [DF_METLWEB_WEBCDRAL] DEFAULT (0) NOT NULL,
    [WEBDSDEC]  VARCHAR (1024) NULL,
    [WEBDSNST]  VARCHAR (1024) CONSTRAINT [DF_METLWEB_WEBDSNST] DEFAULT ('') NOT NULL,
    [WEBDSIMP]  VARCHAR (1024) CONSTRAINT [DF_METLWEB_WEBDSIMP] DEFAULT (1) NOT NULL,
    [WEBWNMTH]  INT            CONSTRAINT [DF_METLWEB_WEBWNMTH] DEFAULT (730) NOT NULL,
    [WEBDSCND]  VARCHAR (1024) CONSTRAINT [DF_METLWEB_WEBDSCND] DEFAULT (1) NOT NULL,
    [WEBSWFRM]  CHAR (1)       CONSTRAINT [DF_METLWEB_WEBSWFRM] DEFAULT ('N') NOT NULL,
    [WEBSWEML]  CHAR (1)       CONSTRAINT [DF_METLWEB_WEBSWEML] DEFAULT ('N') NOT NULL,
    [WEBDSEBO]  VARCHAR (200)  CONSTRAINT [DF_METLWEB_WEBDSEBO] DEFAULT ('') NOT NULL,
    [WEBDSDES]  VARCHAR (300)  CONSTRAINT [DF_METLWEB_WEBDSDES] DEFAULT ('') NOT NULL,
    [WEBDSPCL]  VARCHAR (300)  CONSTRAINT [DF_METLWEB_WEBDSPCL] DEFAULT ('') NOT NULL,
    [WEBSWSSL]  CHAR (1)       CONSTRAINT [DF_METLWEB_WEBSWSSL] DEFAULT ('N') NOT NULL,
    CONSTRAINT [PK_METLWEB] PRIMARY KEY NONCLUSTERED ([WEBINNOD] ASC, [WEBINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLWEB_METLIDI] FOREIGN KEY ([WEBINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLWEB_METLNOD] FOREIGN KEY ([WEBINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLWEB_7_1433824220__K1_12_15]
    ON [dbo].[METLWEB]([WEBINNOD] ASC)
    INCLUDE([WEBDSPLA], [WEBDSLCW]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Herencia dels nodes web i arbre web superiors', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBTPHER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LLista de descripcions de cel·les', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSDEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de valors que indiiquen si es pot imprimir la cel·la o no', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSIMP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de valors que indiiquen si es mostrarà el missatge de contingut no disponible', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSCND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és un formulari ?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBSWFRM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és un correu electrónic? (només per mailing)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBSWEML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estils pel body. Només si no hereta propietats', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSEBO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la pàgina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Paraules clau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBDSPCL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és una pàgina segura?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLWEB', @level2type = N'COLUMN', @level2name = N'WEBSWSSL';

