﻿CREATE TABLE [dbo].[LHFORUSU] (
    [STUSU]    VARCHAR (20)  NOT NULL,
    [STCLA]    VARCHAR (12)  NOT NULL,
    [STNOM]    VARCHAR (200) NOT NULL,
    [STCOG]    VARCHAR (400) NOT NULL,
    [STADR]    VARCHAR (400) NOT NULL,
    [STTIP]    CHAR (1)      CONSTRAINT [DF_LHFORUSU_STTIP] DEFAULT ('N') NOT NULL,
    [DTACS]    DATETIME      NULL,
    [BOBLO]    BIT           CONSTRAINT [DF_LHFORUSU_BOBLO] DEFAULT ('0') NOT NULL,
    [BOELI]    BIT           CONSTRAINT [DF_LHFORUSU_BOELI] DEFAULT ('0') NOT NULL,
    [INAGR]    INT           NULL,
    [USUINESC] INT           NULL,
    [USUDSESC] VARCHAR (50)  NULL,
    [USUINGRU] INT           NULL,
    [USUDSGRU] VARCHAR (60)  NULL,
    CONSTRAINT [PK_LHFORUSU] PRIMARY KEY CLUSTERED ([STUSU] ASC) WITH (FILLFACTOR = 90)
);

