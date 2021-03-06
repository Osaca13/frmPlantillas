﻿CREATE TABLE [dbo].[METLPER] (
    [PERCDREL] NUMERIC (18) NOT NULL,
    [PERCDTIP] INT          NOT NULL,
    [PERCDVER] SMALLINT     NOT NULL,
    [PERINORG] INT          NOT NULL,
    [PERINNOD] INT          NOT NULL,
    [PERINARB] INT          CONSTRAINT [DF_METLPER_PERINARB] DEFAULT (0) NOT NULL,
    [PERCDHER] INT          CONSTRAINT [DF_METLPER_PERCCDHER] DEFAULT (0) NOT NULL,
    [PERCDHE2] INT          CONSTRAINT [DF_METLPER_PERCDHE2] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_METLPER] PRIMARY KEY NONCLUSTERED ([PERCDREL] ASC, [PERCDTIP] ASC, [PERCDVER] ASC, [PERINORG] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLPER_METLREL] FOREIGN KEY ([PERCDREL]) REFERENCES [dbo].[METLREL] ([RELINCOD]) ON DELETE CASCADE NOT FOR REPLICATION
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLPER_7_1424724128__K4_K2_K8_K1]
    ON [dbo].[METLPER]([PERINORG] ASC, [PERCDTIP] ASC, [PERCDHE2] ASC, [PERCDREL] ASC);


GO
CREATE NONCLUSTERED INDEX [METLPER_PERINNOD]
    ON [dbo].[METLPER]([PERINORG] ASC, [PERINNOD] ASC, [PERCDHE2] ASC)
    INCLUDE([PERCDREL], [PERCDTIP]);


GO
CREATE STATISTICS [_dta_stat_1424724128_8_4_2]
    ON [dbo].[METLPER]([PERCDHE2], [PERINORG], [PERCDTIP]);

