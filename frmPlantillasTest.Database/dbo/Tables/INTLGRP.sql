CREATE TABLE [dbo].[INTLGRP] (
    [GRPCDACT] INT NOT NULL,
    [GRPDHANY] INT NOT NULL,
    [GRPCDGRU] INT NOT NULL,
    CONSTRAINT [PK_INTLGRP] PRIMARY KEY CLUSTERED ([GRPCDACT] ASC, [GRPDHANY] ASC, [GRPCDGRU] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de actividad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLGRP', @level2type = N'COLUMN', @level2name = N'GRPCDACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLGRP', @level2type = N'COLUMN', @level2name = N'GRPDHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de grupo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLGRP', @level2type = N'COLUMN', @level2name = N'GRPCDGRU';

