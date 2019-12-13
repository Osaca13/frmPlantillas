CREATE TABLE [dbo].[METLAGD] (
    [AGDINNOD] NUMERIC (18)   NOT NULL,
    [AGDINIDI] SMALLINT       NOT NULL,
    [AGDDSTIT] VARCHAR (500)  NOT NULL,
    [AGDDSRES] TEXT           NULL,
    [AGDDSDES] TEXT           NULL,
    [AGDDTINI] DATETIME       NOT NULL,
    [AGDDTFIN] DATETIME       NULL,
    [AGDINCOL] INT            NULL,
    [AGDINENT] INT            NULL,
    [AGDIMPRE] FLOAT (53)     NULL,
    [AGDDSDUH] INT            NULL,
    [AGDDSDUM] INT            NULL,
    [AGDDSOBS] TEXT           NULL,
    [AGDDTINS] DATETIME       NULL,
    [AGDDTINF] DATETIME       NULL,
    [AGDDSINS] TEXT           NULL,
    [AGDDSEQP] TEXT           NULL,
    [AGDDSORG] TEXT           NULL,
    [AGDDTPUB] DATETIME       NOT NULL,
    [AGDDTCAD] DATETIME       NOT NULL,
    [AGDDSUSR] CHAR (10)      NOT NULL,
    [AGDDSCOM] TEXT           NULL,
    [AGDDTTIM] DATETIME       NULL,
    [AGDSWVIS] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWVIS] DEFAULT (1) NULL,
    [AGDDSHOR] VARCHAR (200)  NULL,
    [AGDSWSUS] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWSUS] DEFAULT (0) NULL,
    [AGDCDJOV] SMALLINT       CONSTRAINT [DF_METLAGD_AGDCDJOV] DEFAULT ((-1)) NULL,
    [AGDSWVAL] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWVAL] DEFAULT (1) NOT NULL,
    [AGDDSSUB] TEXT           NULL,
    [AGDDSFBK] TEXT           NULL,
    [AGDDSLNK] TEXT           NOT NULL,
    [AGDDSVID] VARCHAR (8000) NULL,
    [AGDDSENT] VARCHAR (8000) CONSTRAINT [DF_METLAGD_AGDDSENT] DEFAULT ('') NOT NULL,
    [AGDSWDIA] SMALLINT       NULL,
    [AGDDTHOI] TIME (7)       NULL,
    [AGDDTHOF] TIME (7)       NULL,
    [AGDCDCOX] INT            NULL,
    [AGDCDCOY] INT            NULL,
    [AGDDSGPS] VARCHAR (50)   NULL,
    [AGDDSICO] TEXT           NULL,
    [AGDDSCOA] TEXT           NULL,
    [AGDSWREG] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWREG] DEFAULT ((0)) NULL,
    [AGDSWCSE] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWCSE] DEFAULT ((0)) NULL,
    [AGDSWTEC] SMALLINT       CONSTRAINT [DF_METLAGD_AGDSWTEC] DEFAULT ((0)) NULL,
    [AGDWNASI] SMALLINT       CONSTRAINT [DF_METLAGD_AGDWNASI] DEFAULT ((0)) NULL,
    [AGDWNAFO] SMALLINT       CONSTRAINT [DF_METLAGD_AGDWNAFO] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_METLAGD] PRIMARY KEY CLUSTERED ([AGDINNOD] ASC, [AGDINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLAGD_METLCOL] FOREIGN KEY ([AGDINCOL], [AGDINIDI]) REFERENCES [dbo].[METLCOL] ([COLCDCOL], [COLINIDI]),
    CONSTRAINT [FK_METLAGD_METLENT1] FOREIGN KEY ([AGDINENT], [AGDINIDI]) REFERENCES [dbo].[METLENT] ([ENTCDENT], [ENTINIDI]),
    CONSTRAINT [FK_METLAGD_METLIDI] FOREIGN KEY ([AGDINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLAGD_METLNOD] FOREIGN KEY ([AGDINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
ALTER TABLE [dbo].[METLAGD] NOCHECK CONSTRAINT [FK_METLAGD_METLNOD];


GO
CREATE NONCLUSTERED INDEX [IX_METLAGD]
    ON [dbo].[METLAGD]([AGDINNOD] ASC);


GO
CREATE NONCLUSTERED INDEX [METLAGD_AGDDTPUB_AGDDTCAD]
    ON [dbo].[METLAGD]([AGDDTPUB] ASC, [AGDDTCAD] ASC)
    INCLUDE([AGDINNOD]);


GO
CREATE NONCLUSTERED INDEX [METLAGD_AGDINNOD_AGDDTFIN_AGDDTPUB_AGDDTCAD]
    ON [dbo].[METLAGD]([AGDINIDI] ASC, [AGDSWVIS] ASC, [AGDDTFIN] ASC, [AGDDTPUB] ASC, [AGDDTCAD] ASC)
    INCLUDE([AGDINNOD], [AGDDSTIT], [AGDDTINI], [AGDDTINS]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visible a Internet?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWVIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora única de l''acte. Només s''utilitzarà si tots els dies s''inicia a la mateixa hora.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSHOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Acte suspes?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWSUS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''acte dins de l''aplicació de Joventut.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDCDJOV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç a "valora l''acte"', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWVAL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subtítol de l''acte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSSUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificadors de pàgines de FaceBook on s''ha publicat l''acte, separats per comes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSFBK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç extern de l''acte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSLNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç per a venda d''entrades', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'L''acte dura tot el dia?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWDIA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora d''inici de l''acte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDTHOI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora de final de l''acte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDTHOF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenada X d''Hospigràfic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDCDCOX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenada Y d''Hospigràfic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDCDCOY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenades GPS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSGPS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Institucions col·laboradores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSICO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Com col·labora l''ajuntament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDDSCOA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Prèsencia de regidors a l''acte. 1 si true, 0 si false', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWREG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Prèsencia de caps de servei a l''acte. 1 si true, 0 si false', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWCSE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Prèsencia de tècnics a l''acte. 1 si true, 0 si false', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDSWTEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número d''assistents a l''acte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDWNASI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Aforament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAGD', @level2type = N'COLUMN', @level2name = N'AGDWNAFO';

