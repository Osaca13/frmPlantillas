﻿CREATE TABLE [dbo].[METLBSI] (
    [BSICDLTR] INT           NOT NULL,
    [BSIDSACT] VARCHAR (100) CONSTRAINT [DF_METLBSI_BSIDSACT] DEFAULT ('') NOT NULL,
    [BSIDSAC2] VARCHAR (100) CONSTRAINT [DF_METLBSI_BSIDSAC2] DEFAULT ('') NOT NULL,
    [BSIDSNOV] VARCHAR (100) CONSTRAINT [DF_METLBSI_BSIDSNOV] DEFAULT ('') NOT NULL,
    [BSITPBIB] CHAR (2)      CONSTRAINT [DF_METLBSI_BSITPBIB] DEFAULT ('') NOT NULL,
    [BSIDSTEM] VARCHAR (255) CONSTRAINT [DF_METLBSI_BSIDSTEM] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_METLBSI] PRIMARY KEY CLUSTERED ([BSICDLTR] ASC) WITH (FILLFACTOR = 90)
);

