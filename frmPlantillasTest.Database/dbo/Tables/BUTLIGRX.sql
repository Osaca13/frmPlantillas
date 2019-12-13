﻿CREATE TABLE [dbo].[BUTLIGRX] (
    [IGRXCDPAL] VARCHAR (50)  NOT NULL,
    [IGRXINREL] INT           NOT NULL,
    [IGRXINIDI] SMALLINT      NOT NULL,
    [IGRXCDTIP] VARCHAR (12)  NOT NULL,
    [IGRXDSPOS] VARCHAR (300) NOT NULL,
    [IGRXINNOD] INT           NOT NULL,
    [IGRXDSPNT] FLOAT (53)    NOT NULL,
    [IGRXWSPUB] BIT           NOT NULL,
    [IGRXDHFEC] DATETIME      NOT NULL,
    [IGRXDSFTI] INT           IDENTITY (1, 1) NOT NULL,
    [IGRXDSPNO] FLOAT (53)    CONSTRAINT [DF_BUTLIGRX_IGRXDSPNO] DEFAULT (0) NOT NULL,
    [IGRXSWTIT] BIT           CONSTRAINT [DF_BUTLIGRX_IGRXSWTIT] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BUTLIGRX] PRIMARY KEY CLUSTERED ([IGRXCDPAL] ASC, [IGRXINREL] ASC, [IGRXINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BUTLIGRX_METLIDI] FOREIGN KEY ([IGRXINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);

