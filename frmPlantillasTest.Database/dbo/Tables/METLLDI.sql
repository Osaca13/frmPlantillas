﻿CREATE TABLE [dbo].[METLLDI] (
    [LDIDSEMA] VARCHAR (60)  NOT NULL,
    [LDIINLDI] INT           NOT NULL,
    [LDIDTDNX] DATETIME      NULL,
    [LDIDSNOM] VARCHAR (100) NULL,
    [LDIDSCG1] VARCHAR (100) NULL,
    [LDIDSCG2] VARCHAR (100) NULL,
    [LDIDSPOB] VARCHAR (100) NULL,
    [LDIDSDNI] CHAR (10)     NULL,
    [LDIDTDIN] DATETIME      NULL,
    [LDIDSOBS] VARCHAR (200) NULL,
    CONSTRAINT [PK_METLLDI] PRIMARY KEY CLUSTERED ([LDIDSEMA] ASC, [LDIINLDI] ASC) WITH (FILLFACTOR = 90)
);

