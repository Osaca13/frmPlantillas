CREATE TABLE [dbo].[LHTLREP] (
    [REPCDREP] INT           NOT NULL,
    [REPINRES] INT           NOT NULL,
    [REPCDPRO] INT           NOT NULL,
    [REPDSNOM] VARCHAR (30)  NOT NULL,
    [REPDSCO1] VARCHAR (30)  NOT NULL,
    [REPDSCO2] VARCHAR (30)  NOT NULL,
    [REPTPDNI] CHAR (1)      NOT NULL,
    [REPDSDNI] VARCHAR (20)  NOT NULL,
    [REPCDCAR] INT           NULL,
    [REPDSCAR] VARCHAR (50)  NOT NULL,
    [REPWNCAR] CHAR (4)      NOT NULL,
    [REPDSBIS] CHAR (1)      NULL,
    [REPDSESC] CHAR (2)      NULL,
    [REPDSPIS] CHAR (2)      NULL,
    [REPDSPOR] CHAR (2)      NULL,
    [REPCDCPO] VARCHAR (5)   NOT NULL,
    [REPDSTEL] VARCHAR (20)  NULL,
    [REPDSCOR] VARCHAR (100) NOT NULL,
    [REPINIDI] SMALLINT      CONSTRAINT [DF_LHTLREP_REPINIDI] DEFAULT (1) NOT NULL,
    [REPDSACT] CHAR (1)      CONSTRAINT [DF_LHTLREP_REPDSACT] DEFAULT ('S') NULL,
    [REPINORI] INT           NULL,
    [REPDSALT] VARCHAR (250) NULL,
    [REPININF] INT           NULL,
    CONSTRAINT [PK_LHTLREP] PRIMARY KEY CLUSTERED ([REPCDREP] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_LHTLREP_LHTLPRO] FOREIGN KEY ([REPCDPRO], [REPINIDI]) REFERENCES [dbo].[LHTLPRO] ([PROCDPRO], [PROINIDI]) ON DELETE CASCADE NOT FOR REPLICATION,
    CONSTRAINT [FK_LHTLREP_METLLTR] FOREIGN KEY ([REPCDREP]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero de tramite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPCDREP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de la Promoció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPCDPRO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSCO1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSCO2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de DNI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPTPDNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Document d''identitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSDNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de carrer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPCDCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de carrer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPWNCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSBIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Escala', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSESC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSPIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Porta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSPOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Postal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPCDCPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telèfon', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSTEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu electrònic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPDSCOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LHTLREP', @level2type = N'COLUMN', @level2name = N'REPINIDI';

