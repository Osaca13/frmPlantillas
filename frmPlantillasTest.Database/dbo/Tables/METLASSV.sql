﻿CREATE TABLE [dbo].[METLASSV] (
    [ASSINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [ASSINVER] SMALLINT     NOT NULL,
    [ASSCDTPA] INT          NOT NULL,
    [ASSCDNOD] DECIMAL (18) NOT NULL,
    [ASSCDTIP] INT          NOT NULL,
    [ASSCDNRL] DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_METLASSV] PRIMARY KEY CLUSTERED ([ASSINCOD] ASC, [ASSINVER] ASC) WITH (FILLFACTOR = 90)
);

