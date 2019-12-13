CREATE TABLE [dbo].[TRTLSFO] (
    [SFOINLTR] INT         NOT NULL,
    [SFOCDCUR] INT         NOT NULL,
    [SFODTANY] INT         NULL,
    [SFOSWSEM] CHAR (1)    NULL,
    [SFOCDIDI] SMALLINT    NOT NULL,
    [SFODSCAT] VARCHAR (2) NOT NULL,
    [SFODSFUN] CHAR (70)   NULL,
    [SFODSRLB] CHAR (70)   NULL,
    [SFOWNCUR] CHAR (4)    NULL,
    [SFOSWEXP] CHAR (1)    NOT NULL,
    [SFODSEDS] TEXT        NULL,
    [SFODSOBS] TEXT        NULL,
    [SFOWNPRI] CHAR (3)    NULL,
    [SFOSWNPU] CHAR (1)    NULL,
    CONSTRAINT [PK_TRTLSFO] PRIMARY KEY CLUSTERED ([SFOINLTR] ASC, [SFOCDCUR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLSFO_FORTLCUR] FOREIGN KEY ([SFOCDCUR]) REFERENCES [dbo].[FORTLCUR] ([CURINCOD]) ON DELETE CASCADE,
    CONSTRAINT [FK_TRTLSFO_METLLTR] FOREIGN KEY ([SFOINLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de METLLTR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFOINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de curs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFOCDCUR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma desitjat de resposta. Per defecte 1 (català)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFOCDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Categoria del treballador
Valors: A / B / BC/ D / E 
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFODSCAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lloc de treball', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFODSFUN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Relació laboral', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFODSRLB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Experiència relacionada amb la temàtica sol•licitada. 
S/N (per defecte N).
Categoria del treballador
Valors: A / B / BC/ D / E 
Idioma desitjat de resposta. Per defecte 1 (català)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFOSWEXP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l’experiència relacionada amb la temàtica sol•licitada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFODSEDS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFODSOBS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Prioridad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFO', @level2type = N'COLUMN', @level2name = N'SFOWNPRI';

