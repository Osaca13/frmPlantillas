CREATE TABLE [CPDsa].[LDTLSUB] (
    [SUBINSUB] INT           IDENTITY (1, 1) NOT NULL,
    [SUBINLLD] INT           NOT NULL,
    [SUBINPER] INT           NOT NULL,
    [SUBCDIDI] INT           CONSTRAINT [DF_LDTLSUB_SUBCDIDI] DEFAULT ((-1)) NOT NULL,
    [SUBSWACT] CHAR (1)      CONSTRAINT [DF_LDTLSUB_SUBSWACT] DEFAULT ('S') NOT NULL,
    [SUBDHALT] DATETIME      NOT NULL,
    [SUBDHBXA] DATETIME      NULL,
    [SUBDSMOT] VARCHAR (250) NULL,
    CONSTRAINT [PK_LDTLSUB] PRIMARY KEY CLUSTERED ([SUBINSUB] ASC),
    CONSTRAINT [FK_LDTLSUB_LDTLLLD] FOREIGN KEY ([SUBINLLD]) REFERENCES [CPDsa].[LDTLLLD] ([LLDINLLD]),
    CONSTRAINT [FK_LDTLSUB_LDTLPER] FOREIGN KEY ([SUBINPER]) REFERENCES [CPDsa].[LDTLPER] ([PERINPER])
);


GO
CREATE NONCLUSTERED INDEX [IX_LDTLSUB]
    ON [CPDsa].[LDTLSUB]([SUBINLLD] ASC);


GO
CREATE NONCLUSTERED INDEX [SUBINLLD, SUBINPER]
    ON [CPDsa].[LDTLSUB]([SUBINLLD] ASC, [SUBINPER] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Subscripció', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBINSUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Llista distribució', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBINLLD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Persona', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBINPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Idioma', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBCDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Actiu?', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBSWACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data hora alta', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBDHALT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data hora baixa', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBDHBXA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Motiu baixa', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLSUB', @level2type = N'COLUMN', @level2name = N'SUBDSMOT';

