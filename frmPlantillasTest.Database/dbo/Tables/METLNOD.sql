﻿CREATE TABLE [dbo].[METLNOD] (
    [NODINNOD] NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [NODCDTIP] INT            NOT NULL,
    [NODCDUSR] CHAR (9)       NOT NULL,
    [NODDTTIM] CHAR (20)      NULL,
    [NODDSTXT] VARCHAR (1024) NULL,
    [NODSWVIS] SMALLINT       CONSTRAINT [DF_METLNOD_NODSWVIS] DEFAULT (1) NOT NULL,
    [NODDTTMS] ROWVERSION     NULL,
    [NODSWSIT] SMALLINT       CONSTRAINT [DF_METLNOD_NODSWSIT] DEFAULT ((0)) NULL,
    [NODDTPUB] DATETIME       NULL,
    [NODDTCAD] DATETIME       NULL,
    CONSTRAINT [PK_METLNOD] PRIMARY KEY NONCLUSTERED ([NODINNOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLNOD_METLTIP] FOREIGN KEY ([NODCDTIP]) REFERENCES [dbo].[METLTIP] ([TIPINTIP])
);


GO
CREATE CLUSTERED INDEX [_dta_index_METLNOD_c_7_1801109507__K1_K2]
    ON [dbo].[METLNOD]([NODINNOD] ASC, [NODCDTIP] ASC);


GO
CREATE STATISTICS [_dta_stat_1801109507_1_2]
    ON [dbo].[METLNOD]([NODINNOD], [NODCDTIP]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Si valor= 1, es mostraran als arbres de gaia els continguts amb RELCDSIT=98 --> caducats', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOD', @level2type = N'COLUMN', @level2name = N'NODSWSIT';
