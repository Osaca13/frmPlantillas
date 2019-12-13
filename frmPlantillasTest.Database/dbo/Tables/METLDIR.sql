CREATE TABLE [dbo].[METLDIR] (
    [DIRINNOD]     NUMERIC (18)  NOT NULL,
    [DIRINIDI]     SMALLINT      NOT NULL,
    [DIRDSNOM]     VARCHAR (500) NOT NULL,
    [DIRDSSIG]     VARCHAR (50)  NULL,
    [DIRDSCIF]     VARCHAR (50)  NULL,
    [DIRDSACT]     TEXT          NULL,
    [DIRINCOL]     INT           NULL,
    [DIRINENT]     INT           NULL,
    [DIRIMPRE]     FLOAT (53)    NULL,
    [DIRDSHOR]     TEXT          NULL,
    [DIRDSTEL]     TEXT          NULL,
    [DIRDSOBS]     TEXT          NULL,
    [DIRCDCAR]     INT           NULL,
    [DIRDSCAR]     VARCHAR (250) NULL,
    [DIRDSNUM]     VARCHAR (50)  NULL,
    [DIRDSBIS]     VARCHAR (50)  NULL,
    [DIRDSESC]     VARCHAR (50)  NULL,
    [DIRDSPIS]     VARCHAR (50)  NULL,
    [DIRDSPOR]     VARCHAR (50)  NULL,
    [DIRCDBAR]     INT           NULL,
    [DIRDSBAR]     VARCHAR (250) NULL,
    [DIRCDDIS]     INT           NULL,
    [DIRDSDIS]     VARCHAR (250) NULL,
    [DIRDSCDP]     VARCHAR (5)   NULL,
    [DIRCDMUN]     INT           NULL,
    [DIRDSMUN]     VARCHAR (250) NULL,
    [DIRCDPRV]     INT           NULL,
    [DIRDSPRV]     VARCHAR (250) NULL,
    [DIRCDPAI]     INT           NULL,
    [DIRDSPAI]     VARCHAR (250) NULL,
    [DIRDTPUB]     DATETIME      NOT NULL,
    [DIRDTCAD]     DATETIME      NOT NULL,
    [DIRDTMAN]     DATETIME      NULL,
    [DIRDSPCL]     VARCHAR (500) CONSTRAINT [DF_METLDIR_DIRDSPCL] DEFAULT ('') NULL,
    [DIRWNCON]     BIGINT        CONSTRAINT [DF_METLDIR_DIRINCON] DEFAULT (0) NULL,
    [DIRDSCOM]     TEXT          NULL,
    [DIRCDAIDA1]   TEXT          CONSTRAINT [DF_METLDIR_DIRCDAIDA1] DEFAULT ('') NOT NULL,
    [DIRCDAIDA2]   TEXT          CONSTRAINT [DF_METLDIR_DIRCDAIDA2] DEFAULT ('') NOT NULL,
    [DIRWNTIP]     SMALLINT      CONSTRAINT [DF_METLDIR_DRIWNTIP] DEFAULT (0) NOT NULL,
    [DIRDTALT]     VARCHAR (50)  NULL,
    [DIRDTMOD]     ROWVERSION    NOT NULL,
    [DIRDSWEB]     VARCHAR (255) CONSTRAINT [DF_METLDIR_DIRDSWEB] DEFAULT ('') NULL,
    [DIRDSEML]     VARCHAR (255) CONSTRAINT [DF_METLDIR_DIRDSEML] DEFAULT ('') NULL,
    [DIRDTDIN]     VARCHAR (50)  NULL,
    [DIRDTDAC]     VARCHAR (50)  NULL,
    [DIRDSNRE]     SMALLINT      NULL,
    [DIRDTREG]     VARCHAR (50)  NULL,
    [DIRDSEST]     VARCHAR (20)  NULL,
    [DIRDSAMB]     CHAR (1)      NULL,
    [DIRDSSIT]     CHAR (6)      NULL,
    [DIRDSSOC]     SMALLINT      NULL,
    [DIRDSPRE]     INT           NULL,
    [DIRDSAPR]     SMALLINT      NULL,
    [DIRDSUSU]     VARCHAR (10)  NULL,
    [DIRSWVIS]     SMALLINT      CONSTRAINT [DF_METLDIR_DIRWNVIS] DEFAULT (1) NOT NULL,
    [DIRDSDES]     TEXT          CONSTRAINT [DF_METLDIR_DIRDSDES] DEFAULT ('') NOT NULL,
    [DIRCDCOX]     INT           CONSTRAINT [DF_METLDIR_DIRCDCOX] DEFAULT (0) NULL,
    [DIRCDCOY]     INT           CONSTRAINT [DF_METLDIR_DIRCDCOY] DEFAULT (0) NULL,
    [DIRDSGPS]     VARCHAR (50)  NULL,
    [DIRDSXSO]     VARCHAR (255) NULL,
    [DIRSWACC]     SMALLINT      CONSTRAINT [DF_METLDIR_DIRSWACC] DEFAULT (0) NOT NULL,
    [DIRDSOBS_RSO] TEXT          NULL,
    CONSTRAINT [PK_METLDIR] PRIMARY KEY CLUSTERED ([DIRINNOD] ASC, [DIRINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDIR_7_449488730__K2_K55_K1]
    ON [dbo].[METLDIR]([DIRINIDI] ASC, [DIRSWVIS] ASC, [DIRINNOD] ASC);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[METLDIR]([DIRINIDI] ASC, [DIRDTCAD] ASC)
    INCLUDE([DIRINNOD]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDIR_7_449488730__K2]
    ON [dbo].[METLDIR]([DIRINIDI] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDIR_7_449488730__K2_K1]
    ON [dbo].[METLDIR]([DIRINIDI] ASC, [DIRINNOD] ASC);


GO
CREATE NONCLUSTERED INDEX [METLDIR_DIRINIDI]
    ON [dbo].[METLDIR]([DIRINIDI] ASC)
    INCLUDE([DIRINNOD], [DIRDSNOM]);


GO
CREATE NONCLUSTERED INDEX [DIRINNOD_DIRCDBAR]
    ON [dbo].[METLDIR]([DIRINIDI] ASC, [DIRCDBAR] ASC)
    INCLUDE([DIRINNOD], [DIRDSNOM], [DIRDSGPS]);


GO
CREATE STATISTICS [_dta_stat_449488730_49_2_1]
    ON [dbo].[METLDIR]([DIRDSAMB], [DIRINIDI], [DIRINNOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text lliure amb la descripció de l''horari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSHOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció lliure del telèfon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSTEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''AIDA sidirec.fdinentitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRCDAIDA1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''AIDA sidirec.fdidependencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRCDAIDA2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de entidad: 0:no informado, 1:Registrada, 2:No registrada, 3:jurídica, 4:?, 5:?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRWNTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d''alta de l''entitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDTALT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de modificació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDTMOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’inici d’activitats', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDTDIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’acta ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDTDAC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Núm Registre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSNRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de registre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDTREG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Àmbit territorial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSAMB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Situació local', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSSIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de socis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSSOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pressupost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSPRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Any del pressupost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSAPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visible a Internet?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRSWVIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenades X de l''ubicació de l''equipament ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRCDCOX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenades Y de l''ubicació de l''equipament ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRCDCOY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Xarxes socials', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSXSO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Accessos adaptats per a discapacitats?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRSWACC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció per als equipaments lligats al llistat de recursos socioeconòmics del web de dinamització local.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDIR', @level2type = N'COLUMN', @level2name = N'DIRDSOBS_RSO';

