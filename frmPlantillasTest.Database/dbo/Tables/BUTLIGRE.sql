﻿CREATE TABLE [dbo].[BUTLIGRE] (
    [IGRECDPAL] VARCHAR (50)  NOT NULL,
    [IGREINREL] INT           NOT NULL,
    [IGREINIDI] SMALLINT      NOT NULL,
    [IGRECDTIP] VARCHAR (12)  NOT NULL,
    [IGREDSPOS] VARCHAR (300) NOT NULL,
    [IGREINNOD] INT           NOT NULL,
    [IGREDSPNT] FLOAT (53)    NOT NULL,
    [IGREWSPUB] BIT           NOT NULL,
    [IGREDHFEC] DATETIME      NOT NULL,
    [IGREDSFTI] INT           IDENTITY (1, 1) NOT NULL,
    [IGREDSPNO] FLOAT (53)    CONSTRAINT [DF_BUTLIGRE_IGREDSPNO] DEFAULT (0) NOT NULL,
    [IGRESWTIT] BIT           CONSTRAINT [DF_BUTLIGRE_IGRESWTIT] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_BUTLIGRE] PRIMARY KEY CLUSTERED ([IGRECDPAL] ASC, [IGREINREL] ASC, [IGREINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BUTLIGRE_METLIDI] FOREIGN KEY ([IGREINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);

