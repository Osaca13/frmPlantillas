CREATE TABLE [dbo].[CATLSER] (
    [SERINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [SERINNOM] VARCHAR (40) NULL,
    [SERINACT] CHAR (1)     CONSTRAINT [DF_CATLSER_SERINACT] DEFAULT ('S') NULL,
    CONSTRAINT [PK_CATLSER] PRIMARY KEY CLUSTERED ([SERINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de los Servicios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLSER', @level2type = N'COLUMN', @level2name = N'SERINNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Valor actiu "S", o inactiu "N"', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLSER', @level2type = N'COLUMN', @level2name = N'SERINACT';

