﻿CREATE TABLE [dbo].[FOTLTEM] (
    [TEMINCOD] INT      IDENTITY (1, 1) NOT NULL,
    [TEMDSTIT] TEXT     NOT NULL,
    [TEMDSDSC] TEXT     NULL,
    [TEMDSANU] CHAR (2) NOT NULL,
    CONSTRAINT [PK_FOTLTEM] PRIMARY KEY CLUSTERED ([TEMINCOD] ASC) WITH (FILLFACTOR = 90)
);

