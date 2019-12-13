CREATE TABLE [dbo].[LHFORAGR] (
    [INCOD] INT            IDENTITY (1, 1) NOT NULL,
    [STNOM] VARCHAR (1024) NOT NULL,
    [DTCRE] DATETIME       CONSTRAINT [DF_LHFORAGR_DTCRE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LHFORAGR] PRIMARY KEY CLUSTERED ([INCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHFORAGR', @level2type = N'COLUMN', @level2name = N'DTCRE';

