﻿CREATE TABLE [dbo].[METLIDI] (
    [IDICDIDI] SMALLINT  IDENTITY (1, 1) NOT NULL,
    [IDIDSNOM] CHAR (40) NOT NULL,
    CONSTRAINT [PK_METLIDI] PRIMARY KEY NONCLUSTERED ([IDICDIDI] ASC) WITH (FILLFACTOR = 90)
);

