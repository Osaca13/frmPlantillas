CREATE TABLE [dbo].[METLMAP] (
    [MAPDSHER] VARCHAR (600) NOT NULL,
    [MAPINNOD] INT           CONSTRAINT [DF_METLMAP_MAPINNOD] DEFAULT (0) NOT NULL,
    [MAPDSLPE] VARCHAR (500) NOT NULL,
    [MAPDTTIM] DATETIME      NOT NULL,
    [MAPCDERR] SMALLINT      NULL,
    CONSTRAINT [PK_METLMAP] PRIMARY KEY CLUSTERED ([MAPDSHER] ASC, [MAPINNOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'herencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAP', @level2type = N'COLUMN', @level2name = N'MAPDSHER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llista persones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAP', @level2type = N'COLUMN', @level2name = N'MAPDSLPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data i hora del manteniment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAP', @level2type = N'COLUMN', @level2name = N'MAPDTTIM';

