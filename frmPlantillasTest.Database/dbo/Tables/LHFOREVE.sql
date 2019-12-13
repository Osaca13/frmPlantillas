﻿CREATE TABLE [dbo].[LHFOREVE] (
    [INCOD] INT          IDENTITY (1, 1) NOT NULL,
    [STUSU] VARCHAR (20) NOT NULL,
    [STTIP] CHAR (1)     NOT NULL,
    [STTAU] CHAR (8)     NOT NULL,
    [INELE] INT          NOT NULL,
    [STDES] TEXT         NULL,
    [DTEVE] DATETIME     CONSTRAINT [DF_LHFOREVE_DTEVE] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_LHFOREVE] PRIMARY KEY CLUSTERED ([INCOD] ASC) WITH (FILLFACTOR = 90)
);

