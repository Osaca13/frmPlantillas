CREATE TABLE [dbo].[SLTLELE] (
    [ELCDCOD] INT  IDENTITY (1, 1) NOT NULL,
    [ELDSDES] TEXT NULL,
    CONSTRAINT [PK_SLTLELE] PRIMARY KEY CLUSTERED ([ELCDCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del elemento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLELE', @level2type = N'COLUMN', @level2name = N'ELCDCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLELE', @level2type = N'COLUMN', @level2name = N'ELDSDES';

