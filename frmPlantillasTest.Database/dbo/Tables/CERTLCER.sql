CREATE TABLE [dbo].[CERTLCER] (
    [CERINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [CERDSTXT] VARCHAR (500) NOT NULL,
    [CERWNIDI] SMALLINT      NOT NULL,
    CONSTRAINT [PK_CERTLCER] PRIMARY KEY CLUSTERED ([CERINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_CERTLCER_7_1507536454__K1_K2]
    ON [dbo].[CERTLCER]([CERINCOD] ASC, [CERDSTXT] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de cerca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLCER', @level2type = N'COLUMN', @level2name = N'CERINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cerca realitzada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLCER', @level2type = N'COLUMN', @level2name = N'CERDSTXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'idioma de la cerca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLCER', @level2type = N'COLUMN', @level2name = N'CERWNIDI';

