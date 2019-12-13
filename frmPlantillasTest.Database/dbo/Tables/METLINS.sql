﻿CREATE TABLE [dbo].[METLINS] (
    [INSINACT] NUMERIC (18) NOT NULL,
    [INSINDIR] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_METLINS] PRIMARY KEY CLUSTERED ([INSINACT] ASC, [INSINDIR] ASC) WITH (FILLFACTOR = 90)
);

