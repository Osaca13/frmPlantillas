CREATE TABLE [dbo].[METLDOC] (
    [DOCINNOD] NUMERIC (18)   NOT NULL,
    [DOCINIDI] SMALLINT       CONSTRAINT [DF_METLDOC_DOCINIDI] DEFAULT (1) NOT NULL,
    [DOCINTDO] SMALLINT       NOT NULL,
    [DOCDSTIT] VARCHAR (2000) NOT NULL,
    [DOCDSFIT] VARCHAR (500)  NOT NULL,
    [DOCDSFON] VARCHAR (500)  NULL,
    [DOCDSAUT] TEXT           NULL,
    [DOCDSISB] VARCHAR (500)  NULL,
    [DOCDSDES] TEXT           NULL,
    [DOCDTANY] DATETIME       NULL,
    [DOCDTCAD] DATETIME       NULL,
    [DOCDTPUB] DATETIME       NULL,
    [DOCWNHOR] SMALLINT       CONSTRAINT [DF_METLDOC_DOCWNHOR] DEFAULT (0) NULL,
    [DOCWNVER] SMALLINT       CONSTRAINT [DF_METLDOC_DOCWNVER] DEFAULT (0) NOT NULL,
    [DOCDSLNK] VARCHAR (600)  CONSTRAINT [DF_METLDOC_DOCDSLNK] DEFAULT ('') NOT NULL,
    [DOCDSALT] VARCHAR (1000) CONSTRAINT [DF_METLDOC_DOCDSALT] DEFAULT ('') NOT NULL,
    [DOCWNSIZ] INT            CONSTRAINT [DF_METLDOC_DOCWNSIZ] DEFAULT (0) NOT NULL,
    [DOCDTTIM] ROWVERSION     NOT NULL,
    [DOCWNUPD] SMALLINT       CONSTRAINT [DF_METLDOC_DOCWNUPD] DEFAULT (1) NOT NULL,
    [DOCWNBOR] CHAR (1)       CONSTRAINT [DF_METLDOC_DOCWNBOR] DEFAULT ('N') NOT NULL,
    [DOCDTMOD] DATETIME       NULL,
    CONSTRAINT [PK_METLDOC] PRIMARY KEY CLUSTERED ([DOCINNOD] ASC, [DOCINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLDOC_METLIDI] FOREIGN KEY ([DOCINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLDOC_METLNOD] FOREIGN KEY ([DOCINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD]),
    CONSTRAINT [FK_metldoc_METLTDO] FOREIGN KEY ([DOCINTDO]) REFERENCES [dbo].[METLTDO] ([TDOCDTDO])
);


GO
CREATE NONCLUSTERED INDEX [METLDOC22]
    ON [dbo].[METLDOC]([DOCINIDI] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDOC_7_1074154922__K5]
    ON [dbo].[METLDOC]([DOCDSFIT] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDOC_7_1074154922__K1_K2_K5_4]
    ON [dbo].[METLDOC]([DOCINNOD] ASC, [DOCINIDI] ASC, [DOCDSFIT] ASC)
    INCLUDE([DOCDSTIT]);


GO
CREATE STATISTICS [_dta_stat_1074154922_3_5_11]
    ON [dbo].[METLDOC]([DOCINTDO], [DOCDSFIT], [DOCDTCAD]);


GO
CREATE STATISTICS [_dta_stat_1074154922_2_5_11]
    ON [dbo].[METLDOC]([DOCINIDI], [DOCDSFIT], [DOCDTCAD]);


GO
CREATE STATISTICS [_dta_stat_1074154922_5_2_1_3]
    ON [dbo].[METLDOC]([DOCDSFIT], [DOCINIDI], [DOCINNOD], [DOCINTDO]);


GO
CREATE STATISTICS [_dta_stat_1074154922_5_11]
    ON [dbo].[METLDOC]([DOCDSFIT], [DOCDTCAD]);


GO
CREATE STATISTICS [_dta_stat_1074154922_2_3_5]
    ON [dbo].[METLDOC]([DOCINIDI], [DOCINTDO], [DOCDSFIT]);


GO
CREATE STATISTICS [_dta_stat_1074154922_11_3_2_5_1]
    ON [dbo].[METLDOC]([DOCDTCAD], [DOCINTDO], [DOCINIDI], [DOCDSFIT], [DOCINNOD]);


GO
CREATE STATISTICS [_dta_stat_1074154922_1_5_11_2]
    ON [dbo].[METLDOC]([DOCINNOD], [DOCDSFIT], [DOCDTCAD], [DOCINIDI]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç del document', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDOC', @level2type = N'COLUMN', @level2name = N'DOCDSLNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text alternatiu. Descripció de la imatge', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDOC', @level2type = N'COLUMN', @level2name = N'DOCDSALT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Necessita publicar-se de nou?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDOC', @level2type = N'COLUMN', @level2name = N'DOCWNUPD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Esborrar de la carpeta original?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDOC', @level2type = N'COLUMN', @level2name = N'DOCWNBOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data d''actualització', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDOC', @level2type = N'COLUMN', @level2name = N'DOCDTMOD';

