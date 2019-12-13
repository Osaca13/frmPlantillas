﻿CREATE TABLE [dbo].[BUTLIPR] (
    [IPRINNOD] NCHAR (10)  NOT NULL,
    [IPRDSNOM] NCHAR (250) NOT NULL,
    [IPRDSCO1] NCHAR (250) NOT NULL,
    [IPRDSCO2] NCHAR (250) NOT NULL,
    [IPRDSCAR] NCHAR (250) NOT NULL,
    [IPRDSTEL] NCHAR (250) NOT NULL,
    [IPRDSEML] NCHAR (250) NOT NULL,
    [IPRDSPON] NCHAR (250) NOT NULL,
    [IPRDSNEG] NCHAR (250) NOT NULL,
    [IPRDSEDI] NCHAR (250) NOT NULL,
    [IPRDSPIS] NCHAR (250) NOT NULL,
    [IPRDSADR] NCHAR (250) NOT NULL,
    CONSTRAINT [PK_BUTLIPR2] PRIMARY KEY CLUSTERED ([IPRINNOD] ASC)
);


GO
CREATE NONCLUSTERED INDEX [BUTLIPR_IPRSDSPIS]
    ON [dbo].[BUTLIPR]([IPRDSPIS] ASC)
    INCLUDE([IPRINNOD]);


GO
CREATE NONCLUSTERED INDEX [BUTLIPR_IPRDSPON]
    ON [dbo].[BUTLIPR]([IPRDSPON] ASC)
    INCLUDE([IPRINNOD]);


GO
CREATE NONCLUSTERED INDEX [BUTLIPR_IPRDSEDI]
    ON [dbo].[BUTLIPR]([IPRDSEDI] ASC)
    INCLUDE([IPRINNOD]);

