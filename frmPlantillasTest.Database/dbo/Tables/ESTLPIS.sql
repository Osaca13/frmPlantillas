CREATE TABLE [dbo].[ESTLPIS] (
    [PISINPIS] INT            IDENTITY (1, 1) NOT NULL,
    [PISININS] INT            NOT NULL,
    [PISDSCAT] VARCHAR (50)   NOT NULL,
    [PISDSCAS] VARCHAR (50)   NOT NULL,
    [PISWNORD] SMALLINT       CONSTRAINT [DF_ESTLPIS_PISINORD] DEFAULT ((0)) NOT NULL,
    [PISTIMIN] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTAIMIN] DEFAULT ((27.0)) NOT NULL,
    [PISTIMAX] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTAIMAX] DEFAULT ((31.0)) NOT NULL,
    [PISTEMIN] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTAMMIN] DEFAULT ((28.0)) NOT NULL,
    [PISTEMAX] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTAMMAX] DEFAULT ((32.0)) NOT NULL,
    [PISCLMIN] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISCLOMIN] DEFAULT ((0.5)) NOT NULL,
    [PISCLMAX] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISCLOMAX] DEFAULT ((3.0)) NOT NULL,
    [PISPHMIN] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISPHMIN] DEFAULT ((7.0)) NOT NULL,
    [PISPHMAX] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISPHMAX] DEFAULT ((7.8)) NOT NULL,
    [PISCOMIN] DECIMAL (4, 1) CONSTRAINT [DF_ESTLPIS_PISCOMIN] DEFAULT ((0)) NULL,
    [PISCOMAX] DECIMAL (4, 1) CONSTRAINT [DF_ESTLPIS_PISCOMAX] DEFAULT ((0)) NULL,
    [PISTRMIN] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTRMIN] DEFAULT ((0)) NOT NULL,
    [PISTRMAX] DECIMAL (3, 1) CONSTRAINT [DF_ESTLPIS_PISTRMAX] DEFAULT ((20)) NOT NULL,
    CONSTRAINT [PK_ESTLPIS] PRIMARY KEY CLUSTERED ([PISINPIS] ASC),
    CONSTRAINT [FK_ESTLPIS_ESTLINS] FOREIGN KEY ([PISININS]) REFERENCES [dbo].[ESTLINS] ([INSININS])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Piscina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLPIS', @level2type = N'COLUMN', @level2name = N'PISINPIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Instal·lació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLPIS', @level2type = N'COLUMN', @level2name = N'PISININS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció piscina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLPIS', @level2type = N'COLUMN', @level2name = N'PISDSCAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ordre d''aparició', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESTLPIS', @level2type = N'COLUMN', @level2name = N'PISWNORD';

