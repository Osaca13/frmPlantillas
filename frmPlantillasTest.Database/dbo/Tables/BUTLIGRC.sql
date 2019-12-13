﻿CREATE TABLE [dbo].[BUTLIGRC] (
    [IGRCCDPAL] VARCHAR (50)  NOT NULL,
    [IGRCINREL] INT           NOT NULL,
    [IGRCINIDI] SMALLINT      NOT NULL,
    [IGRCCDTIP] VARCHAR (12)  NOT NULL,
    [IGRCDSPOS] VARCHAR (300) NOT NULL,
    [IGRCINNOD] INT           NOT NULL,
    [IGRCDSPNT] FLOAT (53)    NOT NULL,
    [IGRCWSPUB] BIT           NOT NULL,
    [IGRCDHFEC] DATETIME      NOT NULL,
    [IGRCDSFTI] INT           IDENTITY (1, 1) NOT NULL,
    [IGRCDSPNO] FLOAT (53)    CONSTRAINT [DF_BUTLIGRC_IGRCDSPNO] DEFAULT (0) NOT NULL,
    [IGRCSWTIT] BIT           CONSTRAINT [DF_BUTLIGRC_IGRCSWTIT] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BUTLIGRC] PRIMARY KEY CLUSTERED ([IGRCCDPAL] ASC, [IGRCINREL] ASC, [IGRCINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BUTLIGRC_METLIDI] FOREIGN KEY ([IGRCINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);

