CREATE TABLE [dbo].[TRTLMAP] (
    [MAPCDLTR] INT           NOT NULL,
    [MAPSWGR1] CHAR (1)      CONSTRAINT [DF_TRTLMAP_MAPSWGR1] DEFAULT ('N') NOT NULL,
    [MAPSWGR2] CHAR (1)      CONSTRAINT [DF_TRTLMAP_MAPSWGR2] DEFAULT ('N') NOT NULL,
    [MAPSWGR3] CHAR (1)      CONSTRAINT [DF_TRTLMAP_MAPSWGR3] DEFAULT ('N') NOT NULL,
    [MAPSWGR4] CHAR (1)      CONSTRAINT [DF_TRTLMAP_MAPSWGR4] DEFAULT ('N') NOT NULL,
    [MAPSWGR5] CHAR (1)      CONSTRAINT [DF_TRTLMAP_MAPSWGR5] DEFAULT ('N') NOT NULL,
    [MAPDSOBS] VARCHAR (300) NOT NULL,
    CONSTRAINT [PK_TRTLMAP] PRIMARY KEY CLUSTERED ([MAPCDLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLMAP_METLLTR] FOREIGN KEY ([MAPCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grup de mobilitat sostenible, territori i transport públic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLMAP', @level2type = N'COLUMN', @level2name = N'MAPSWGR1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grup d''energies renovables, aigua, residus i contaminació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLMAP', @level2type = N'COLUMN', @level2name = N'MAPSWGR2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grup 3. Grup d''educació mediambiental i participació ciutadana', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLMAP', @level2type = N'COLUMN', @level2name = N'MAPSWGR3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'observacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLMAP', @level2type = N'COLUMN', @level2name = N'MAPDSOBS';

