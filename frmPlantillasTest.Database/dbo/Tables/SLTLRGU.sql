﻿CREATE TABLE [dbo].[SLTLRGU] (
    [RGUINLTR] INT           NOT NULL,
    [RGUDSNIF] CHAR (21)     NULL,
    [RGUSTNOM] VARCHAR (150) NOT NULL,
    [RGUSTDTN] VARCHAR (10)  NULL,
    [RGUSTTLF] VARCHAR (9)   NULL,
    [RGUSTLLT] VARCHAR (150) CONSTRAINT [DF_SLTLRGU_RGUSTLLT] DEFAULT (' ') NULL,
    [RGUSTDEP] TEXT          NOT NULL,
    [RGUSTTEL] VARCHAR (16)  NOT NULL,
    [RGUSTCOR] VARCHAR (100) NOT NULL,
    [RGUINTIP] INT           NOT NULL,
    CONSTRAINT [PK_SLTLRGU] PRIMARY KEY CLUSTERED ([RGUINLTR] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_SLTLRGU_7_279060130__K2_K1]
    ON [dbo].[SLTLRGU]([RGUDSNIF] ASC, [RGUINLTR] ASC);

