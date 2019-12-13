﻿CREATE TABLE [dbo].[METLASS] (
    [ASSINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [ASSCDTPA] INT          NOT NULL,
    [ASSCDNOD] NUMERIC (18) NOT NULL,
    [ASSCDTIP] INT          NOT NULL,
    [ASSCDNRL] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_METLASS] PRIMARY KEY CLUSTERED ([ASSINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLASS_7_1652916960__K2_K5_K3]
    ON [dbo].[METLASS]([ASSCDTPA] ASC, [ASSCDNRL] ASC, [ASSCDNOD] ASC);


GO
CREATE STATISTICS [_dta_stat_1652916960_3_5_2]
    ON [dbo].[METLASS]([ASSCDNOD], [ASSCDNRL], [ASSCDTPA]);

