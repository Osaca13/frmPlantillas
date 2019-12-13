CREATE TABLE [dbo].[SLTLACC] (
    [ACCINLTR]  INT           NOT NULL,
    [ACCDSUTR]  CHAR (100)    NOT NULL,
    [ACCDSEDI]  CHAR (70)     NOT NULL,
    [ACCDSRLB]  CHAR (70)     NOT NULL,
    [ACCDSCAT]  VARCHAR (2)   NOT NULL,
    [ACCDSLTR]  CHAR (70)     NOT NULL,
    [ACCDSCAR]  CHAR (10)     NOT NULL,
    [ACCDSNCD]  CHAR (70)     NOT NULL,
    [ACCTMACC]  DATETIME      NULL,
    [ACCINHOR]  INT           NULL,
    [ACCDSINT]  CHAR (2)      NULL,
    [ACCDTACC]  DATETIME      NULL,
    [ACCDSLLOC] CHAR (100)    NULL,
    [ACCDSACT]  TEXT          NULL,
    [ACCSWFHA]  CHAR (1)      NULL,
    [ACCINDUR]  INT           NULL,
    [ACCDSCOS]  CHAR (100)    NULL,
    [ACCDSCAU]  TEXT          NULL,
    [ACCDSTIP]  TEXT          NULL,
    [ACCSWBXA]  CHAR (1)      NULL,
    [ACCDSCEN]  TEXT          NULL,
    [ACCSWQIX]  CHAR (1)      NULL,
    [ACCDSQIX]  TEXT          NULL,
    [ACCDSNTE]  VARCHAR (260) NULL,
    [ACCDSDMA]  TEXT          NULL,
    [ACCSWREP]  CHAR (1)      NULL,
    [ACCSWGRA]  CHAR (1)      NULL,
    [ACCDSCOM]  TEXT          NULL,
    [ACCDSCAUS] TEXT          NULL,
    [ACCDSMES]  TEXT          NULL,
    [ACCDSRESP] VARCHAR (260) NULL,
    [ACCDHREV]  CHAR (10)     NULL,
    [ACCDSTES]  TEXT          NULL,
    [ACCDSINV]  TEXT          NULL,
    [ACCDHCUR]  DATETIME      NULL,
    CONSTRAINT [PK_SLTLACC] PRIMARY KEY CLUSTERED ([ACCINLTR] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de METLLTR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unitat de Treball', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSUTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Centre de treball. Edifici', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSEDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de contracte. Relació laboral', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSRLB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Categoria', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ocupación professional. Lloc de treball', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Càrrec del comandament directe', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del comandament directe', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSNCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora del accidente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCTMACC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora dentro de la  jornada laboral', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCINHOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In itínere : AN TO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSINT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data del accident', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDTACC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lloc de l''accident', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSLLOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Quina activitat feia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Era feina habitual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCSWFHA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Quant temps fa que realitza aquesta activitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCINDUR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Part del cos lesionada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCOS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Que va causar el accident', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCAU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de lesió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ha causat baixa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCSWBXA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Centre sanitari on ha estat atès', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tens alguna queixa (S/N)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCSWQIX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la queixa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSQIX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom i cognoms del testimoni', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSNTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del danys materials ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSDMA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Possibilitat de que es repetixi l''accident: 1: Molt baixa;2:Ocasional;3:Freqüent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCSWREP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Gravetat potencial de l''accident.1: Lleu;2: Greu;3:Molt greu', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCSWGRA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de COM es va produir l''accident', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de QUINES CAUSES van produir l''accident', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSCAUS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de QUINES MESURES per evitar-ho ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSMES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Responsable de gestionar les mesures anteriors', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSRESP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de revisió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDHREV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom i telèfon de les persones entrevistades(accidentat, testimonis, etc...)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSTES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Investigat per: nom del responsable, comandament o assimilat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDSINV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLACC', @level2type = N'COLUMN', @level2name = N'ACCDHCUR';

