﻿CREATE TABLE [dbo].[METLCOL] (
    [COLCDCOL] INT           NOT NULL,
    [COLINIDI] SMALLINT      CONSTRAINT [DF_METLCOL_COLINIDI] DEFAULT (1) NOT NULL,
    [COLDSDES] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_METLCOL] PRIMARY KEY CLUSTERED ([COLCDCOL] ASC, [COLINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLCOL_METLIDI] FOREIGN KEY ([COLINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);

