﻿CREATE TABLE [dbo].[METLASSV_REVISIO] (
    [ASSINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [ASSINVER] SMALLINT     NOT NULL,
    [ASSCDTPA] INT          NOT NULL,
    [ASSCDNOD] DECIMAL (18) NOT NULL,
    [ASSCDTIP] INT          NOT NULL,
    [ASSCDNRL] DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_METLASSV_REVISIO] PRIMARY KEY CLUSTERED ([ASSINCOD] ASC, [ASSINVER] ASC) WITH (FILLFACTOR = 90)
);
