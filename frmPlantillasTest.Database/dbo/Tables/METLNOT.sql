CREATE TABLE [dbo].[METLNOT] (
    [NOTINNOD] NUMERIC (18)   NOT NULL,
    [NOTINIDI] SMALLINT       NOT NULL,
    [NOTDSTIT] VARCHAR (1000) NOT NULL,
    [NOTDSSUB] VARCHAR (1000) NOT NULL,
    [NOTDSRES] TEXT           CONSTRAINT [DF_METLNOT_NOTDSRES] DEFAULT ('') NOT NULL,
    [NOTDSTXT] TEXT           NOT NULL,
    [NOTDTANY] SMALLDATETIME  NOT NULL,
    [NOTDTPUB] SMALLDATETIME  NOT NULL,
    [NOTDTCAD] SMALLDATETIME  NOT NULL,
    [NOTDSUSR] CHAR (20)      NOT NULL,
    [NOTDTTIM] CHAR (20)      NULL,
    [NOTDSAVN] VARCHAR (1000) CONSTRAINT [DF_METLNOT_NOTDSAVN] DEFAULT ('') NOT NULL,
    [NOTDSFBK] TEXT           NULL,
    [NOTDSLNK] TEXT           NOT NULL,
    [NOTDSVID] VARCHAR (8000) NULL,
    CONSTRAINT [PK_METLNOT] PRIMARY KEY NONCLUSTERED ([NOTINNOD] ASC, [NOTINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLNOT_METLIDI] FOREIGN KEY ([NOTINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLNOT_METLNOD] FOREIGN KEY ([NOTINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLNOT_7_794537964__K2_K1]
    ON [dbo].[METLNOT]([NOTINIDI] ASC, [NOTINNOD] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subtítol de la notícia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOT', @level2type = N'COLUMN', @level2name = N'NOTDSSUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOT', @level2type = N'COLUMN', @level2name = N'NOTDSRES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOT', @level2type = N'COLUMN', @level2name = N'NOTDSAVN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificadors de pàgina FB on s''ha publicat la notícia, separats per comes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOT', @level2type = N'COLUMN', @level2name = N'NOTDSFBK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç extern de la notícia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOT', @level2type = N'COLUMN', @level2name = N'NOTDSLNK';

