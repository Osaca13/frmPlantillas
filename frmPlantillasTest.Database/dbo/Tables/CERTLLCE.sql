CREATE TABLE [dbo].[CERTLLCE] (
    [LCEINCOD] INT           NOT NULL,
    [LCECDNOD] INT           NOT NULL,
    [LCEDSLNK] VARCHAR (300) NOT NULL,
    [LCEDSDAT] DATETIME      NOT NULL,
    [LCEWNCON] INT           NOT NULL,
    CONSTRAINT [FK_CERTLLCE_CERTLCER] FOREIGN KEY ([LCEINCOD]) REFERENCES [dbo].[CERTLCER] ([CERINCOD])
);


GO
CREATE NONCLUSTERED INDEX [CERTLLCE9]
    ON [dbo].[CERTLLCE]([LCEINCOD] ASC, [LCECDNOD] ASC, [LCEDSDAT] ASC, [LCEWNCON] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de cerca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLLCE', @level2type = N'COLUMN', @level2name = N'LCEINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de node GAIA on es troba el contingut', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLLCE', @level2type = N'COLUMN', @level2name = N'LCECDNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'link al contingut acceptat per l''usuari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLLCE', @level2type = N'COLUMN', @level2name = N'LCEDSLNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data de la última cerca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLLCE', @level2type = N'COLUMN', @level2name = N'LCEDSDAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'comptador de les vegades que s''ha acceptat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CERTLLCE', @level2type = N'COLUMN', @level2name = N'LCEWNCON';

