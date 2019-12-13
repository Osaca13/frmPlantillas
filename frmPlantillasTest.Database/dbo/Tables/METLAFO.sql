﻿CREATE TABLE [dbo].[METLAFO] (
    [AFOINNOD] NUMERIC (18) NOT NULL,
    [AFOINCOL] INT          NOT NULL,
    [AFODTDAT] DATETIME     NOT NULL,
    [AFODSPEU] INT          NULL,
    [AFODSSEN] INT          NULL,
    [AFODSVIP] INT          NULL,
    [AFODSGRU] INT          NULL,
    [AFOSWILI] CHAR (1)     NOT NULL,
    [AFOINIDI] SMALLINT     CONSTRAINT [DF_METLAFO_AFOINIDI] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_METLAFO] PRIMARY KEY CLUSTERED ([AFOINNOD] ASC, [AFOINCOL] ASC, [AFODTDAT] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLAFO_METLAGD] FOREIGN KEY ([AFOINNOD], [AFOINIDI]) REFERENCES [dbo].[METLAGD] ([AGDINNOD], [AGDINIDI]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_METLAFO_METLNOD] FOREIGN KEY ([AFOINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma. Sempre català', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAFO', @level2type = N'COLUMN', @level2name = N'AFOINIDI';

