﻿CREATE TABLE [CPDsa].[METLHOR2] (
    [HORCDCOD] INT          IDENTITY (1, 1) NOT NULL,
    [HORINNOD] NUMERIC (18) NOT NULL,
    [HORDTINI] DATE         NOT NULL,
    [HORDTFIN] DATE         NOT NULL,
    [HORDHINI] VARCHAR (7)  NOT NULL,
    [HORDHFIN] VARCHAR (7)  NOT NULL,
    [HORWNDES] INT          NOT NULL,
    [HORWNPRC] INT          NOT NULL,
    [HORWNPRE] INT          NOT NULL,
    [HORWNAFO] SMALLINT     CONSTRAINT [DF_METLHOR2_HORWNAFO] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED ([HORCDCOD] ASC),
    CONSTRAINT [FK_METLHOR_METLNOD] FOREIGN KEY ([HORINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Aforament', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLHOR2', @level2type = N'COLUMN', @level2name = N'HORWNAFO';

