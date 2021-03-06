﻿CREATE TABLE [dbo].[METLTIP] (
    [TIPINTIP] INT           IDENTITY (1, 1) NOT NULL,
    [TIPCDVER] INT           NOT NULL,
    [TIPDSDES] CHAR (30)     NOT NULL,
    [TIPCDUSR] CHAR (9)      NOT NULL,
    [TIPDTTIM] ROWVERSION    NOT NULL,
    [TIPDSIMG] VARCHAR (200) NULL,
    CONSTRAINT [PK_METLTIP] PRIMARY KEY NONCLUSTERED ([TIPINTIP] ASC) WITH (FILLFACTOR = 90)
);

