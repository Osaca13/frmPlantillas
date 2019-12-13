CREATE TABLE [dbo].[METLDPR] (
    [DPRCDLTR] INT           NOT NULL,
    [DPRDSDNI] CHAR (10)     NOT NULL,
    [DPRDSCG1] VARCHAR (100) NULL,
    [DPRDSCG2] VARCHAR (100) NULL,
    [DPRDSNOM] VARCHAR (100) NULL,
    [DPRDSTL1] VARCHAR (20)  NULL,
    [DPRDSTL2] VARCHAR (20)  NULL,
    [DPRDSDNX] CHAR (10)     NULL,
    [DPRDSEML] VARCHAR (60)  NULL,
    [DPRDSADR] VARCHAR (200) CONSTRAINT [DF_METLDPR_DPRDSADR] DEFAULT ('') NULL,
    [DPRDSCPS] VARCHAR (5)   CONSTRAINT [DF_METLDPR_DPRDSCPS] DEFAULT ('') NULL,
    [DPRDSPBL] VARCHAR (50)  CONSTRAINT [DF_METLDPR_DPRDSPBL] DEFAULT ('') NULL,
    [DPRTPELI] CHAR (1)      CONSTRAINT [DF_METLDPR_DPRTPELI] DEFAULT ('N') NULL,
    [DPRDSOBS] VARCHAR (300) CONSTRAINT [DF_METLDPR_DPRDSOBS] DEFAULT ('') NULL,
    [DPRINPER] INT           CONSTRAINT [DF_METLDPR_DPRINPER] DEFAULT (0) NULL,
    [DPRWNSEX] SMALLINT      CONSTRAINT [DF_METLDPR_DPRWNSEX] DEFAULT (0) NULL,
    [DPRDSPRO] VARCHAR (150) NULL,
    [DPRDSTRE] VARCHAR (100) NULL,
    [DPRDSENT] VARCHAR (100) NULL,
    [DPRTPIDI] SMALLINT      CONSTRAINT [DF_METLDPR_DPRTPIDI] DEFAULT (1) NULL,
    CONSTRAINT [PK_METLDPR] PRIMARY KEY CLUSTERED ([DPRCDLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLDPR_METLLTR] FOREIGN KEY ([DPRCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [METLDPR5]
    ON [dbo].[METLDPR]([DPRCDLTR] ASC, [DPRDSDNI] ASC, [DPRDSCG1] ASC, [DPRDSCG2] ASC, [DPRDSNOM] ASC, [DPRDSTL1] ASC, [DPRDSEML] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLDPR_7_175391744__K2]
    ON [dbo].[METLDPR]([DPRDSDNI] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DNI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSDNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSCG1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cognom 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSCG2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telèfon 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSTL1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telèfon 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSTL2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de naixement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSDNX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e-mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSEML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSADR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi postal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSCPS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Població', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSPBL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'L’usuari ha demanat esborrar les seves dades personals un cop finalitzada la tramitació
“S”: Sí
“N”: No
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRTPELI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions/altres dades', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSOBS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de treballador de l’Ajuntament. 

', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRINPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sexe: 0:no informat, 1:home, 2:dona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRWNSEX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Professió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSPRO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Càrrec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSTRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lloc de treball / Entitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRDSENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma de resposta desitjat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLDPR', @level2type = N'COLUMN', @level2name = N'DPRTPIDI';

