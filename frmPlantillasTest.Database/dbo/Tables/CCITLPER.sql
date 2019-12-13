﻿CREATE TABLE [dbo].[CCITLPER] (
    [PERINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [PERTPDOC] CHAR (1)      CONSTRAINT [DF_CCITLPER_PERTPDOC] DEFAULT ('N') NOT NULL,
    [PERSTDOC] VARCHAR (21)  NOT NULL,
    [PERSTNOM] VARCHAR (250) NOT NULL,
    [PERSTCOG] VARCHAR (100) NOT NULL,
    [PERSTCO1] VARCHAR (50)  NOT NULL,
    [PERSTCO2] VARCHAR (50)  NOT NULL,
    [PERDTCRE] DATETIME      CONSTRAINT [DF_CCITLPER_PERDTCRE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__CCITLPER__468862B0] PRIMARY KEY CLUSTERED ([PERINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLPER', @level2type = N'COLUMN', @level2name = N'PERSTCO1';

