﻿CREATE TABLE [dbo].[BUTLIGRU] (
    [IGRUCDPAL] VARCHAR (50)  NOT NULL,
    [IGRUINREL] INT           NOT NULL,
    [IGRUINIDI] SMALLINT      NOT NULL,
    [IGRUCDTIP] VARCHAR (12)  NOT NULL,
    [IGRUDSPOS] VARCHAR (300) NOT NULL,
    [IGRUINNOD] INT           NOT NULL,
    [IGRUDSPNT] FLOAT (53)    NOT NULL,
    [IGRUWSPUB] BIT           NOT NULL,
    [IGRUDHFEC] DATETIME      NOT NULL,
    [IGRUDSFTI] INT           IDENTITY (1, 1) NOT NULL,
    [IGRUDSPNO] FLOAT (53)    CONSTRAINT [DF_BUTLIGRU_IGRUDSPNO] DEFAULT (0) NOT NULL,
    [IGRUSWTIT] BIT           CONSTRAINT [DF_BUTLIGRU_IGRUSWTIT] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BUTLIGRU] PRIMARY KEY CLUSTERED ([IGRUCDPAL] ASC, [IGRUINREL] ASC, [IGRUINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BUTLIGRU_METLIDI] FOREIGN KEY ([IGRUINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);

