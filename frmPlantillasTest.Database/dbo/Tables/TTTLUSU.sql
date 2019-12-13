﻿CREATE TABLE [dbo].[TTTLUSU] (
    [USUINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [USUDSNOM] VARCHAR (50) NULL,
    [USUDSPAS] VARCHAR (50) NULL,
    [USUINACT] INT          NULL,
    CONSTRAINT [PK_TTTLUSU] PRIMARY KEY CLUSTERED ([USUINCOD] ASC) WITH (FILLFACTOR = 90)
);

