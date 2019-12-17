﻿CREATE TABLE [dbo].[BUTLIDO] (
    [IDOINNOD] INT      NOT NULL,
    [IDOINIDI] SMALLINT NOT NULL,
    [IDODHFEC] DATETIME NOT NULL,
    [IDOCDSIT] INT      NOT NULL,
    [IDODHVIS] DATETIME NOT NULL,
    CONSTRAINT [PK_BUTLIDO] PRIMARY KEY CLUSTERED ([IDOINNOD] ASC, [IDOINIDI] ASC) WITH (FILLFACTOR = 90)
);
