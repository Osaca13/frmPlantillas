﻿CREATE TABLE [dbo].[METLENT] (
    [ENTCDENT] INT       NOT NULL,
    [ENTINIDI] SMALLINT  NOT NULL,
    [ENTDSNOM] CHAR (50) NOT NULL,
    [ENTDSDES] TEXT      NULL,
    CONSTRAINT [PK_METLENT] PRIMARY KEY CLUSTERED ([ENTCDENT] ASC, [ENTINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLENT_METLIDI] FOREIGN KEY ([ENTINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);


GO
CREATE NONCLUSTERED INDEX [IX_METLENT]
    ON [dbo].[METLENT]([ENTCDENT] ASC) WITH (FILLFACTOR = 90);

