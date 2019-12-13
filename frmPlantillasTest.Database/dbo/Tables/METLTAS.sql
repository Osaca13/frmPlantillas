CREATE TABLE [dbo].[METLTAS] (
    [TASINTAS] INT           IDENTITY (1, 1) NOT NULL,
    [TASCDTAS] INT           NOT NULL,
    [TASDSPLA] VARCHAR (255) NULL,
    [TASDSCON] VARCHAR (MAX) NULL,
    [TASDSLOG] VARCHAR (255) NULL,
    [TASDSTRA] VARCHAR (255) NULL,
    [TASINTRA] INT           NULL,
    [TASDSFIT] VARCHAR (255) NULL,
    [TASDSDNI] VARCHAR (9)   NULL,
    [TASDSMAI] VARCHAR (255) NULL,
    [TASCDIDI] SMALLINT      NULL,
    [TASDSASU] VARCHAR (255) NULL,
    [TASDSRES] VARCHAR (255) NULL,
    [TASINPER] INT           NULL,
    [TASWNREG] INT           NULL,
    [TASDHREG] DATETIME2 (7) NULL,
    [TASDSORI] VARCHAR (255) NULL,
    [TASDHCRE] DATETIME2 (7) NULL,
    [TASSWFET] SMALLINT      CONSTRAINT [DF__METLTAS__TASSWFE__76280F4A] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_METLTAS] PRIMARY KEY CLUSTERED ([TASINTAS] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = N'Identificador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASINTAS';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Codi tasca', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASCDTAS';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Plantilla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSPLA';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Contingut', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSCON';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Fitxer log', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSLOG';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Nom tramit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSTRA';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Codi tramit Gaia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASINTRA';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Nom fitxer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSFIT';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Dni', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSDNI';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSMAI';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASCDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Assumpte mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSASU';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Mail responsable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSRES';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'IdPersonaCC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASINPER';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Num Registre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASWNREG';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Timestamp registre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDHREG';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Origen', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDSORI';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Data creacio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASDHCRE';


GO
EXECUTE sp_addextendedproperty @name = N'Descripcio', @value = 'Processat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLTAS', @level2type = N'COLUMN', @level2name = N'TASSWFET';

