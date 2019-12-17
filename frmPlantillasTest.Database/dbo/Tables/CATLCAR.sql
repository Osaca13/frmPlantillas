﻿CREATE TABLE [dbo].[CATLCAR] (
    [CARCDCAR] CHAR (4)  NOT NULL,
    [CARDSTPN] CHAR (16) NOT NULL,
    [CARDSNOM] CHAR (30) NOT NULL,
    [CARDSTPA] CHAR (2)  NULL,
    [CARDSNAB] CHAR (16) NOT NULL,
    CONSTRAINT [PK_CATLCAR] PRIMARY KEY CLUSTERED ([CARCDCAR] ASC) WITH (FILLFACTOR = 90)
);
