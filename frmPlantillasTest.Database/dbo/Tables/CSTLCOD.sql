CREATE TABLE [dbo].[CSTLCOD] (
    [CODWNCOD] INT           IDENTITY (1, 1) NOT NULL,
    [CODTPCOD] CHAR (2)      NOT NULL,
    [CODDSCOD] VARCHAR (500) NULL,
    [CODDSLNK] VARCHAR (300) NULL,
    [CODDSCOR] VARCHAR (300) NULL,
    [CODDSESP] VARCHAR (50)  NULL,
    [CODDSESF] VARCHAR (50)  NULL,
    [CODDSTIP] CHAR (1)      NULL,
    [CODDSCO2] VARCHAR (300) NULL,
    CONSTRAINT [PK_CSTLCOD] PRIMARY KEY NONCLUSTERED ([CODWNCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CSTLCOD_CSTLTCO] FOREIGN KEY ([CODTPCOD]) REFERENCES [dbo].[CSTLTCO] ([TCOINCOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça on hi ha el backoffice del tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLCOD', @level2type = N'COLUMN', @level2name = N'CODDSLNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de correus separats per “;” dels usuaris responsables del tràmit que rebran avisos de que tenen tràmits pendents', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLCOD', @level2type = N'COLUMN', @level2name = N'CODDSCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de codis d’estat "pendent" separats per “,”.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLCOD', @level2type = N'COLUMN', @level2name = N'CODDSESP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de codis d’estat "finalitzat" separats per “,”.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLCOD', @level2type = N'COLUMN', @level2name = N'CODDSESF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu de destí de les respostes que genera el tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CSTLCOD', @level2type = N'COLUMN', @level2name = N'CODDSCO2';

