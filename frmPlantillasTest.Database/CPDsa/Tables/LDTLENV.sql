CREATE TABLE [CPDsa].[LDTLENV] (
    [ENVINENV] INT           IDENTITY (1, 1) NOT NULL,
    [ENVINLLD] INT           NOT NULL,
    [ENVDSRTE] VARCHAR (100) NOT NULL,
    [ENVDSTMA] VARCHAR (250) NOT NULL,
    [ENVTXCON] TEXT          NOT NULL,
    [ENVINOBE] INT           NOT NULL,
    [ENVTXINF] TEXT          NOT NULL,
    [ENVDHENV] DATETIME      NULL,
    [ENVWNENV] SMALLINT      CONSTRAINT [DF_LDTLENV_ENVWNENV] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LDTLENV] PRIMARY KEY CLUSTERED ([ENVINENV] ASC),
    CONSTRAINT [FK_LDTLENV_LDTLLLD] FOREIGN KEY ([ENVINLLD]) REFERENCES [CPDsa].[LDTLLLD] ([LLDINLLD])
);


GO
CREATE NONCLUSTERED INDEX [IX_LDTLENV]
    ON [CPDsa].[LDTLENV]([ENVINLLD] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Enviament', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVINENV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Llista distribució', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVINLLD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remitent', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVDSRTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tema', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVDSTMA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contingut', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVTXCON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contador mails oberts', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVINOBE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Informació correus oberts', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVTXINF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data hora enviament', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVDHENV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contador mails enviats', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLENV', @level2type = N'COLUMN', @level2name = N'ENVWNENV';

