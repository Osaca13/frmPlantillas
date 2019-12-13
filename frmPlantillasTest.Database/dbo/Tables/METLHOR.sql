﻿CREATE TABLE [dbo].[METLHOR] (
    [HORINNOD] NUMERIC (18) NOT NULL,
    [HORDHDAT] DATETIME     NOT NULL,
    [HORDSHFI] VARCHAR (5)  NULL,
    [HORDSDUH] INT          NULL,
    [HORDSDUM] INT          NULL,
    [HORDSDES] VARCHAR (80) NULL,
    CONSTRAINT [PK_METLHOR] PRIMARY KEY CLUSTERED ([HORINNOD] ASC, [HORDHDAT] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLHOR_METLNOD] FOREIGN KEY ([HORINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD]),
    CONSTRAINT [IX_METLHOR] UNIQUE NONCLUSTERED ([HORINNOD] ASC, [HORDHDAT] ASC, [HORDSHFI] ASC) WITH (FILLFACTOR = 90)
);

