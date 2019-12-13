﻿CREATE TABLE [dbo].[METLNTR] (
    [NTRINNOD] NUMERIC (18) NOT NULL,
    [NTRINIDI] INT          NOT NULL,
    [NTRDSTXT] TEXT         NULL,
    CONSTRAINT [PK_METLNTR] PRIMARY KEY CLUSTERED ([NTRINNOD] ASC, [NTRINIDI] ASC) WITH (FILLFACTOR = 90)
);

