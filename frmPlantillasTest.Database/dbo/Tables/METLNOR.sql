CREATE TABLE [dbo].[METLNOR] (
    [NORINNOD] INT            NOT NULL,
    [NORINIDI] SMALLINT       NOT NULL,
    [NORDSCOD] TEXT           NOT NULL,
    [NORDSTIT] TEXT           NOT NULL,
    [NORDTANY] ROWVERSION     NOT NULL,
    [NORDSUSR] CHAR (10)      NOT NULL,
    [NORDSGAD] VARCHAR (1000) CONSTRAINT [DF_METLNOR_NORDSGAD] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_METLNOR] PRIMARY KEY NONCLUSTERED ([NORINNOD] ASC, [NORINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grup d''active directori', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNOR', @level2type = N'COLUMN', @level2name = N'NORDSGAD';

