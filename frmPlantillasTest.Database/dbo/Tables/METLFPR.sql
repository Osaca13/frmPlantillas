CREATE TABLE [dbo].[METLFPR] (
    [FPRINNOD] NUMERIC (18)  NOT NULL,
    [FPRINIDI] SMALLINT      NOT NULL,
    [FPRDSNOM] VARCHAR (500) NOT NULL,
    [FPRDSDES] TEXT          NOT NULL,
    [FPRDSRES] TEXT          NULL,
    [FPRDSOPR] TEXT          NULL,
    [FPRDSDOB] TEXT          NULL,
    [FPRDSFIN] TEXT          NULL,
    [FPRWNCUM] SMALLINT      CONSTRAINT [DF_METLFPR_FPRWNCUM] DEFAULT (0) NOT NULL,
    [FPRDSPER] VARCHAR (300) CONSTRAINT [DF_METLFPR_FPRDSPER] DEFAULT ('Període del projecte. Descripció amb text del període d’execució del projecte. Possibles retards. etc..') NULL,
    [FPRDTINI] DATETIME      NOT NULL,
    [FPRDTFIN] DATETIME      NOT NULL,
    [FPRWNCOM] SMALLINT      CONSTRAINT [DF_METLFPR_FPRWNCOM] DEFAULT (0) NOT NULL,
    [FPRCDCAR] VARCHAR (4)   NULL,
    [FPRDSCAR] VARCHAR (500) NULL,
    [FPRDSNUM] VARCHAR (50)  NULL,
    [FPRDSBIS] VARCHAR (50)  NULL,
    [FPRDSESC] VARCHAR (50)  NULL,
    [FPRDSPIS] VARCHAR (50)  NULL,
    [FPRDSPOR] VARCHAR (50)  NULL,
    [FPRCDBAR] SMALLINT      NULL,
    [FPRCDDIS] SMALLINT      NULL,
    [FPRDSCDP] VARCHAR (5)   NULL,
    [FPRCDCOX] NUMERIC (10)  NULL,
    [FPRCDCOY] NUMERIC (10)  NULL,
    [FPRDSLOC] TEXT          NULL,
    [FPRDTPUB] DATETIME      NOT NULL,
    [FPRDTCAD] DATETIME      NOT NULL,
    [FPRDSUSR] VARCHAR (10)  NOT NULL,
    [FPRDSCOM] TEXT          NULL,
    [FPRDTTIM] DATETIME      NULL,
    [FPRSWVIS] SMALLINT      NOT NULL,
    [FPRDSDCO] TEXT          NULL,
    CONSTRAINT [PK_METLFPR] PRIMARY KEY CLUSTERED ([FPRINNOD] ASC, [FPRINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRINNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Títol del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del projecte.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Resum del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSRES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de compliment del projecte. 
Per defecte=0 (sense començar)
Valors de 0 a 10. 
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRWNCUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’inici del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDTINI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de fi del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDTFIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'En comercialització? 0: no, 1:si', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRWNCOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del carrer d’hospitalet on es realitza (si és una actuació urbanística)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRCDCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del carrer d’hospitalet on es realitza (si és una actuació urbanística)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número d’edifici', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSNUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSBIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Escala', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSESC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSPIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Porta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSPOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del barri', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRCDBAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del districte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRCDDIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenada X', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRCDCOX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Coordenada Y', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRCDCOY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de publicació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDTPUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de caducitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDTCAD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nif del creador del projecte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSUSR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions d’ús intern ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSCOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Timestamp ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDTTIM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFPR', @level2type = N'COLUMN', @level2name = N'FPRDSDCO';

