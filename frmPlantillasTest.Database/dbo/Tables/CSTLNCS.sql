CREATE TABLE [dbo].[CSTLNCS] (
    [NCSINCOD] INT  NOT NULL,
    [NCSINIDI] INT  NOT NULL,
    [NCSDSNOM] TEXT NOT NULL,
    CONSTRAINT [PK_CSTLNCS] PRIMARY KEY CLUSTERED ([NCSINCOD] ASC, [NCSINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLNCS', @level2type = N'COLUMN', @level2name = N'NCSINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLNCS', @level2type = N'COLUMN', @level2name = N'NCSINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLNCS', @level2type = N'COLUMN', @level2name = N'NCSDSNOM';

