CREATE TABLE [CPDsa].[METLMAP2] (
    [MAPINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [MAPINNOD] NUMERIC (18)  NOT NULL,
    [MAPCDREL] NUMERIC (18)  NOT NULL,
    [MAPDSLPE] VARCHAR (500) NOT NULL,
    [MAPDTTIM] DATETIME      NOT NULL,
    [MAPCDERR] SMALLINT      NOT NULL,
    CONSTRAINT [PK_Tabla1] PRIMARY KEY CLUSTERED ([MAPINCOD] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llista persones', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLMAP2', @level2type = N'COLUMN', @level2name = N'MAPCDREL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llista persones', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLMAP2', @level2type = N'COLUMN', @level2name = N'MAPDSLPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data i hora del manteniment', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLMAP2', @level2type = N'COLUMN', @level2name = N'MAPDTTIM';

