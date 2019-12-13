﻿CREATE TABLE [dbo].[TTTLDAT] (
    [DATINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [DATINUSU] INT           NULL,
    [DATINCAT] INT           NULL,
    [DATDTINI] SMALLDATETIME NULL,
    [DATDTFIN] SMALLDATETIME NULL,
    CONSTRAINT [PK_TTTLDAT] PRIMARY KEY CLUSTERED ([DATINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TTTLDAT_TTTLCAT] FOREIGN KEY ([DATINCAT]) REFERENCES [dbo].[TTTLCAT] ([CATINCOD]) ON UPDATE CASCADE,
    CONSTRAINT [FK_TTTLDAT_TTTLUSU] FOREIGN KEY ([DATINUSU]) REFERENCES [dbo].[TTTLUSU] ([USUINCOD]) ON UPDATE CASCADE
);

