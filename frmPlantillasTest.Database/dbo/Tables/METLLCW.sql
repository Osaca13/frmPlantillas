CREATE TABLE [dbo].[METLLCW] (
    [LCWINNOD] NUMERIC (18)   NOT NULL,
    [LCWINIDI] SMALLINT       NOT NULL,
    [LCWDSTIT] TEXT           NOT NULL,
    [LCWDTANY] ROWVERSION     NOT NULL,
    [LCWDSUSR] CHAR (10)      NOT NULL,
    [LCWDSTXT] TEXT           NOT NULL,
    [LCWCDTIP] SMALLINT       CONSTRAINT [DF_METLLCW_LCWCDTIP] DEFAULT (1) NOT NULL,
    [LCWTPFOR] CHAR (1)       CONSTRAINT [DF_METLLCW_LCWPTFOR] DEFAULT ('N') NOT NULL,
    [LCWTPFOL] CHAR (1)       NULL,
    [LCWDSHLP] VARCHAR (2000) CONSTRAINT [DF_METLLCW_LCWDSHLP] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_METLLCW] PRIMARY KEY NONCLUSTERED ([LCWINNOD] ASC, [LCWINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLLCW_METLIDI] FOREIGN KEY ([LCWINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLLCW_METLIDI1] FOREIGN KEY ([LCWINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI]),
    CONSTRAINT [FK_METLLCW_METLNOD] FOREIGN KEY ([LCWINNOD]) REFERENCES [dbo].[METLNOD] ([NODINNOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de la llibreria de codi', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWINNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma del codi web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Títol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWDSTIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de creació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWDTANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del usuari creador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWDSUSR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text amb el codi web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWDSTXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de codi web', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWCDTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forzar aparició del codi si no hi ha contingut?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLCW', @level2type = N'COLUMN', @level2name = N'LCWTPFOR';

