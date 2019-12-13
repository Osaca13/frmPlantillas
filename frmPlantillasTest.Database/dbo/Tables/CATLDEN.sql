CREATE TABLE [dbo].[CATLDEN] (
    [DENINCOD] INT             IDENTITY (1, 1) NOT NULL,
    [DENDSDNI] CHAR (5)        NULL,
    [DENINSER] INT             NULL,
    [DENINPAI] INT             CONSTRAINT [DF_CATLDEN_DENINPAI] DEFAULT (0) NULL,
    [DENDSPAI] NVARCHAR (50)   NULL,
    [DENDTDEN] SMALLDATETIME   NULL,
    [DENINNAI] INT             NULL,
    [DENINLAB] INT             CONSTRAINT [DF_CATLDEN_DENINLAB] DEFAULT (0) NULL,
    [DENINHIJ] INT             CONSTRAINT [DF_CATLDEN_DENINHIJ] DEFAULT (0) NULL,
    [DENINRAG] INT             CONSTRAINT [DF_CATLDEN_DENINRAG] DEFAULT (0) NULL,
    [DENINCAG] INT             CONSTRAINT [DF_CATLDEN_DENINCAG] DEFAULT (0) NULL,
    [DENINTIP] INT             CONSTRAINT [DF_CATLDEN_DENINTIP] DEFAULT (0) NULL,
    [DENINCAI] VARCHAR (300)   NULL,
    [DENDSOBS] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_CATLDEN] PRIMARY KEY CLUSTERED ([DENINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CATLDEN_CATLSER] FOREIGN KEY ([DENINSER]) REFERENCES [dbo].[CATLSER] ([SERINCOD]) ON UPDATE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'4 últims digits del DNI i la lletra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENDSDNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau foranea CATLSER', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINSER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau foranea de CATLPAI (un o es que no te pais)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINPAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de la denuncia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENDTDEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Any de naixement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINNAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Situacio laboral (0 No treballa,1 Si treballa)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINLAB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de fills(5 vol dir 5 o mes fills)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINHIJ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Relació amb l''agressor (0 Parella, 1 Ex-Parella, 2 Familiar)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINRAG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Conviu amb l''agressor (0 NO, 1 SI)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINCAG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de Violencia (0 Fisica,1 Emocional,2 Sexual)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'La llista de serveis separats -', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENINCAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CATLDEN', @level2type = N'COLUMN', @level2name = N'DENDSOBS';

