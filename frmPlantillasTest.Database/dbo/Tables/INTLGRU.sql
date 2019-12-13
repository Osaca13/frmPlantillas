CREATE TABLE [dbo].[INTLGRU] (
    [GRUINGRU] INT       NOT NULL,
    [GRUDSNOM] CHAR (60) NOT NULL,
    CONSTRAINT [PK_INTLGRU] PRIMARY KEY CLUSTERED ([GRUINGRU] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de grupo escolar', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLGRU', @level2type = N'COLUMN', @level2name = N'GRUINGRU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del grupo escolar', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLGRU', @level2type = N'COLUMN', @level2name = N'GRUDSNOM';

