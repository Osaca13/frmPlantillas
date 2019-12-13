CREATE TABLE [dbo].[CCITLUSU] (
    [USUSTUSU] VARCHAR (21)  NOT NULL,
    [USUSTCLA] VARCHAR (8)   NOT NULL,
    [USUINPER] INT           NOT NULL,
    [USUSTCOR] VARCHAR (255) NOT NULL,
    [USUININT] INT           CONSTRAINT [DF_CCITLUSU_USUININT] DEFAULT (0) NOT NULL,
    [USUDTINT] DATETIME      NULL,
    [USUDTBLO] DATETIME      NULL,
    [USUBOBAI] CHAR (1)      CONSTRAINT [DF_CCITLUSU_USUBOBAI] DEFAULT ('N') NOT NULL,
    [USUWNIDI] CHAR (2)      NULL,
    CONSTRAINT [PK__CCITLUSU__4964CF5B] PRIMARY KEY CLUSTERED ([USUSTUSU] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK__CCITLUSU__USUINP__4B4D17CD] FOREIGN KEY ([USUINPER]) REFERENCES [dbo].[CCITLPER] ([PERINCOD]) ON DELETE CASCADE,
    CONSTRAINT [UQ__CCITLUSU__4A58F394] UNIQUE NONCLUSTERED ([USUINPER] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'document d''identitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUSTUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUSTCLA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Persona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUINPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu electrónic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUSTCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero d''intents d''accés erronis avui', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUININT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data ultim intent erroni', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUDTINT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data últim bloqueig', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUDTBLO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari donat de baixa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLUSU', @level2type = N'COLUMN', @level2name = N'USUBOBAI';

