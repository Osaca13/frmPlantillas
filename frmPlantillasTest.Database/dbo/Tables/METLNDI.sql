﻿CREATE TABLE [dbo].[METLNDI] (
    [NDIINNOD] INT        NOT NULL,
    [NDICDIDI] SMALLINT   NOT NULL,
    [NDIDSNOM] TEXT       NOT NULL,
    [NDIDTANY] ROWVERSION NOT NULL,
    CONSTRAINT [PK_METLNDI] PRIMARY KEY CLUSTERED ([NDIINNOD] ASC, [NDICDIDI] ASC) WITH (FILLFACTOR = 90)
);

