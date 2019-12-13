CREATE TABLE [dbo].[FORTLCUR] (
    [CURINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [CURCDARE] INT           NOT NULL,
    [CURDSNOM] VARCHAR (100) NOT NULL,
    [CURSWCUR] CHAR (1)      CONSTRAINT [DF_FORTLCUR_CURSWCUR] DEFAULT ('S') NOT NULL,
    [CURDTFIN] DATETIME      NULL,
    [CURDTANY] INT           NULL,
    [CURWNSEM] CHAR (1)      NULL,
    CONSTRAINT [PK_FORTLCUR] PRIMARY KEY CLUSTERED ([CURINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FORTLCUR_FORTLARE] FOREIGN KEY ([CURCDARE]) REFERENCES [dbo].[FORTLARE] ([AREINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Formació. Codi de curs. Autonumèric.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLCUR', @level2type = N'COLUMN', @level2name = N'CURINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d’àrea', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLCUR', @level2type = N'COLUMN', @level2name = N'CURCDARE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del curs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLCUR', @level2type = N'COLUMN', @level2name = N'CURDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Curs actiu. Per defecte S', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FORTLCUR', @level2type = N'COLUMN', @level2name = N'CURSWCUR';

