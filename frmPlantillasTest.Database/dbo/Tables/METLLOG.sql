﻿CREATE TABLE [dbo].[METLLOG] (
    [LOGINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [LOGCDREL] NUMERIC (18) NOT NULL,
    [LOGDTDAT] DATETIME     NOT NULL,
    [LOGTPTIP] CHAR (10)    NOT NULL,
    [LOGCDUSR] CHAR (50)    NOT NULL,
    [LOGDSTXT] TEXT         NOT NULL,
    [LOGINNOD] INT          CONSTRAINT [DF_METLLOG_LOGINNOD] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_METLLOG] PRIMARY KEY NONCLUSTERED ([LOGINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [METLLOG3]
    ON [dbo].[METLLOG]([LOGDTDAT] ASC, [LOGCDREL] ASC, [LOGTPTIP] ASC, [LOGINNOD] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [LOGCDUSR_LOGINNOD]
    ON [dbo].[METLLOG]([LOGCDUSR] ASC, [LOGINNOD] ASC)
    INCLUDE([LOGINCOD], [LOGTPTIP]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLOG', @level2type = N'COLUMN', @level2name = N'LOGINNOD';

