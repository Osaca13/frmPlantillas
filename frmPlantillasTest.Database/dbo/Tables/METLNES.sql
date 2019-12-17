﻿CREATE TABLE [dbo].[METLNES] (
    [NESINNOD] INT        NOT NULL,
    [NESINIDI] SMALLINT   NOT NULL,
    [NESDSTIT] TEXT       NOT NULL,
    [NESCDORG] INT        NOT NULL,
    [NESDTANY] ROWVERSION NOT NULL,
    [NESDSUSR] CHAR (10)  NOT NULL,
    CONSTRAINT [PK_METLNES] PRIMARY KEY NONCLUSTERED ([NESINNOD] ASC, [NESINIDI] ASC) WITH (FILLFACTOR = 90)
);
