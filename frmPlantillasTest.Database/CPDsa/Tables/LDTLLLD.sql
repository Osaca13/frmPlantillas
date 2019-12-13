CREATE TABLE [CPDsa].[LDTLLLD] (
    [LLDINLLD] INT           IDENTITY (1, 1) NOT NULL,
    [LLDDSNOM] VARCHAR (150) NOT NULL,
    [LLDDSRTE] VARCHAR (100) NOT NULL,
    [LLDDTBXA] DATE          NULL,
    CONSTRAINT [PK_LDTLLLD] PRIMARY KEY CLUSTERED ([LLDINLLD] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Llista distribució', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLLLD', @level2type = N'COLUMN', @level2name = N'LLDINLLD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLLLD', @level2type = N'COLUMN', @level2name = N'LLDDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remitent', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLLLD', @level2type = N'COLUMN', @level2name = N'LLDDSRTE';

