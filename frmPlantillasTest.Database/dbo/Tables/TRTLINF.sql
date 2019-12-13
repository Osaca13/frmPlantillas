CREATE TABLE [dbo].[TRTLINF] (
    [INFCDLTR] INT            NOT NULL,
    [INFDSRES] VARCHAR (500)  CONSTRAINT [DF_TRTLINF_INFDSRES] DEFAULT ('') NOT NULL,
    [INFDSTXT] VARCHAR (5000) CONSTRAINT [DF_TRTLINF_INFDSTXT] DEFAULT ('') NOT NULL,
    [INFCDIDI] SMALLINT       CONSTRAINT [DF_TRTLINF_INFCDIDI] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_TRTLINF] PRIMARY KEY CLUSTERED ([INFCDLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLINF_METLLTR] FOREIGN KEY ([INFCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Objecte de la consulta / Breu resum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLINF', @level2type = N'COLUMN', @level2name = N'INFDSRES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la sol•licitud d’informació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLINF', @level2type = N'COLUMN', @level2name = N'INFDSTXT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma desitjat de resposta ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLINF', @level2type = N'COLUMN', @level2name = N'INFCDIDI';

