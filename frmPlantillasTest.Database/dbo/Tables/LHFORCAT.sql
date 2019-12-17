﻿CREATE TABLE [dbo].[LHFORCAT] (
    [INCOD] INT            IDENTITY (1, 1) NOT NULL,
    [INAGR] INT            NOT NULL,
    [STNOM] VARCHAR (8000) NOT NULL,
    [DTCRE] DATETIME       CONSTRAINT [DF_LHFORCAT_DTCRE] DEFAULT (getdate()) NOT NULL,
    [STUSU] VARCHAR (20)   NOT NULL,
    [BOBLO] BIT            CONSTRAINT [DF_LHFORCAT_BOBLO] DEFAULT ('0') NOT NULL,
    [BOELI] BIT            CONSTRAINT [DF_LHFORCAT_BOELI] DEFAULT ('0') NOT NULL,
    CONSTRAINT [PK_LHFORCAT] PRIMARY KEY CLUSTERED ([INCOD] ASC) WITH (FILLFACTOR = 90)
);
