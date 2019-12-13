CREATE TABLE [dbo].[ESTLANA] (
    [ANAINANA] INT            IDENTITY (1, 1) NOT NULL,
    [ANAINPIS] INT            NOT NULL,
    [ANADHANA] DATETIME       NOT NULL,
    [ANAWNTAI] DECIMAL (3, 1) NOT NULL,
    [ANAWNTAM] DECIMAL (3, 1) NOT NULL,
    [ANAWNCLO] DECIMAL (3, 1) NOT NULL,
    [ANAWNNPH] DECIMAL (3, 1) NOT NULL,
    [ANAWNCO2] DECIMAL (4, 1) DEFAULT ((0)) NULL,
    [ANAWNCLC] DECIMAL (3, 1) DEFAULT ((0)) NOT NULL,
    [ANAWNTER] DECIMAL (3, 1) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ESTLANA] PRIMARY KEY CLUSTERED ([ANAINANA] ASC),
    CONSTRAINT [FK_ESTLANA_ESTLPIS] FOREIGN KEY ([ANAINPIS]) REFERENCES [dbo].[ESTLPIS] ([PISINPIS])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Analítica', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAINANA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Piscina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAINPIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data hora analítica', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANADHANA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Temperatura aigua', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAWNTAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Temperatura ambient', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAWNTAM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clor i/o brom', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAWNCLO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell PH', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLANA', @level2type = N'COLUMN', @level2name = N'ANAWNNPH';

