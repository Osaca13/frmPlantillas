CREATE TABLE [dbo].[CSTLTIP] (
    [TIPINCOD] INT           NOT NULL,
    [TIPCDEST] SMALLINT      NOT NULL,
    [TIPDSNOM] VARCHAR (50)  NOT NULL,
    [TIPDSDES] VARCHAR (200) NOT NULL,
    [TIPWNFIN] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_CSTLTIP] PRIMARY KEY CLUSTERED ([TIPINCOD] ASC, [TIPCDEST] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CSTLTIP_CSTLCOD] FOREIGN KEY ([TIPINCOD]) REFERENCES [dbo].[CSTLCOD] ([CODWNCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de tràmit a la taula CSTLCOD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLTIP', @level2type = N'COLUMN', @level2name = N'TIPINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLTIP', @level2type = N'COLUMN', @level2name = N'TIPCDEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de l''estat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLTIP', @level2type = N'COLUMN', @level2name = N'TIPDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l''estat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLTIP', @level2type = N'COLUMN', @level2name = N'TIPDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estat Final?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLTIP', @level2type = N'COLUMN', @level2name = N'TIPWNFIN';

