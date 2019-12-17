﻿CREATE TABLE [dbo].[BUTLIGRI] (
    [IGRICDPAL] VARCHAR (50)  NOT NULL,
    [IGRIINREL] INT           NOT NULL,
    [IGRIINIDI] SMALLINT      NOT NULL,
    [IGRICDTIP] VARCHAR (12)  NOT NULL,
    [IGRIDSPOS] VARCHAR (300) NOT NULL,
    [IGRIINNOD] INT           NOT NULL,
    [IGRIDSPNT] FLOAT (53)    NOT NULL,
    [IGRIWSPUB] BIT           NOT NULL,
    [IGRIDHFEC] DATETIME      NOT NULL,
    [IGRIDSFTI] INT           IDENTITY (1, 1) NOT NULL,
    [IGRIDSPNO] FLOAT (53)    CONSTRAINT [DF_BUTLIGRI_IGRIDSPNO] DEFAULT (0) NOT NULL,
    [IGRISWTIT] BIT           CONSTRAINT [DF_BUTLIGRI_IGRISWTIT] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BUTLIGRI] PRIMARY KEY CLUSTERED ([IGRICDPAL] ASC, [IGRIINREL] ASC, [IGRIINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BUTLIGRI_METLIDI] FOREIGN KEY ([IGRIINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);
