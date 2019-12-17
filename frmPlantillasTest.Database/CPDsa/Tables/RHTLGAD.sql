﻿CREATE TABLE [CPDsa].[RHTLGAD] (
    [GADINCOD] INT         IDENTITY (1, 1) NOT NULL,
    [GADDSGRP] NCHAR (150) NULL,
    [GADDSIPS] NCHAR (150) NULL,
    CONSTRAINT [PK_RHTLGAD] PRIMARY KEY CLUSTERED ([GADINCOD] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_RHTLGAD]
    ON [CPDsa].[RHTLGAD]([GADINCOD] ASC);
