CREATE TABLE [dbo].[METLFTR] (
    [FTRINNOD] NUMERIC (10)   NOT NULL,
    [FTRINIDI] SMALLINT       NOT NULL,
    [FTRDSNOM] VARCHAR (500)  NOT NULL,
    [FTRDSEEX] TEXT           NULL,
    [FTRDSDES] TEXT           NULL,
    [FTRDSQUI] TEXT           NULL,
    [FTRDSHOW] TEXT           NULL,
    [FTRDSPRS] TEXT           NULL,
    [FTRDSTEL] TEXT           NULL,
    [FTRDSWEB] TEXT           NULL,
    [FTRDSQUA] VARCHAR (8000) NULL,
    [FTRDSOBS] TEXT           NULL,
    [FTRDSPRE] TEXT           NULL,
    [FTRDSTRE] TEXT           NULL,
    [FTRDSLEG] TEXT           NULL,
    [FTRDSALE] VARCHAR (80)   NULL,
    [FTRDTALI] DATETIME       CONSTRAINT [DF_METLFTR_FTRDTALI] DEFAULT ('1/1/1900') NULL,
    [FTRDTALF] DATETIME       CONSTRAINT [DF_METLFTR_FTRDTALF] DEFAULT ('1/1/1900') NOT NULL,
    [FTRDTPUB] DATETIME       NULL,
    [FTRDTCAD] DATETIME       NULL,
    [FTRDTMAN] DATETIME       NULL,
    [FTRDSPCL] VARCHAR (500)  NULL,
    [FTRDSCOM] TEXT           NULL,
    [FTRSWVIS] CHAR (1)       CONSTRAINT [DF_METLFTR_FTRSWVIS] DEFAULT ('N') NULL,
    [FTRDSCOR] VARCHAR (150)  NULL,
    [FTRINUNI] INT            NULL,
    [FTRSWCAR] CHAR (1)       CONSTRAINT [DF_METLFTR_FTRSWCAR] DEFAULT (0) NULL,
    [FTRCDTRA] SMALLINT       NULL,
    [FTRCDRTE] VARCHAR (20)   NULL,
    [FTRDSFAJ] VARCHAR (8000) NULL,
    [FTRDSFDO] VARCHAR (8000) NULL,
    [FTRSWAOC] CHAR (1)       NULL,
    [FTRCDSIL] CHAR (1)       NULL,
    [FTRDSDEC] VARCHAR (8000) NULL,
    [FTRCDORG] INT            NULL,
    [FTRSWNOT] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWNOT] DEFAULT ('-1') NULL,
    [FTRSWTPV] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWTPV] DEFAULT ('-1') NULL,
    [FTRSWTEM] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWTEM] DEFAULT ('-1') NULL,
    [FTRSWREG] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWREG] DEFAULT ('-1') NULL,
    [FTRSWIDN] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWIDN] DEFAULT ('-1') NULL,
    [FTRDSTEC] TEXT           NULL,
    [FTRDSOBJ] TEXT           NULL,
    [FTRDSETP] TEXT           NULL,
    [FTRDSIND] TEXT           NULL,
    [FTRDSTER] TEXT           NULL,
    [FTRDSTDO] TEXT           NULL,
    [FTRDSPDO] TEXT           NULL,
    [FTRSWVUD] CHAR (2)       CONSTRAINT [DF_METLFTR_FTRSWVUD] DEFAULT ('-1') NULL,
    [FTRDSQDC] TEXT           NULL,
    [FTRDSAPD] TEXT           NULL,
    [FTRSWVSE] CHAR (1)       CONSTRAINT [DF_METLFTR_FTRSWVSE] DEFAULT ('0') NULL,
    [FTRSWVIN] CHAR (1)       CONSTRAINT [DF_METLFTR_FTRSWVIN] DEFAULT ('0') NULL,
    [FTRSWVWE] CHAR (1)       CONSTRAINT [DF_METLFTR_FTRSWVEW] DEFAULT ('0') NULL,
    [FTRDSFWE] VARCHAR (100)  NULL,
    [FTRCDTTE] VARCHAR (20)   CONSTRAINT [DF_METLFTR_FTRCDTTE] DEFAULT ('') NULL,
    [FTRCDTTT] VARCHAR (20)   CONSTRAINT [DF_METLFTR_FTRCDTTT] DEFAULT ('') NULL,
    [FTRCDTTS] VARCHAR (20)   CONSTRAINT [DF_METLFTR_FTRCDTTS] DEFAULT ('') NULL,
    [FTRCDUTS] SMALLINT       CONSTRAINT [DF_METLFTR_FTRCDUTS] DEFAULT ((0)) NULL,
    [FTRDSNOC] VARCHAR (100)  NULL,
    [FTRDSVAL] TEXT           NULL,
    [FTRDSRAP] TEXT           NULL,
    [FTRDSRAT] TEXT           NULL,
    [FTRDSRAI] TEXT           NULL,
    [FTRDSDFG] TEXT           NULL,
    [FTRDSDIP] TEXT           NULL,
    [FTRDSDIT] TEXT           NULL,
    [FTRDSDII] TEXT           NULL,
    [FTRWNNSV] INT            CONSTRAINT [DF_METLFTR_FTRDSNSV] DEFAULT ((0)) NULL,
    [FTRWNNS1] INT            CONSTRAINT [DF_METLFTR_FTRDSNS1] DEFAULT ((0)) NULL,
    [FTRWNNS2] INT            CONSTRAINT [DF_METLFTR_FTRDSNS2] DEFAULT ((0)) NULL,
    [FTRWNNS3] INT            CONSTRAINT [DF_METLFTR_FTRDSNS3] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_METLFTR2] PRIMARY KEY CLUSTERED ([FTRINNOD] ASC, [FTRINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLFTR_7_1834541669__K1_K2_K24_K52]
    ON [dbo].[METLFTR]([FTRINNOD] ASC, [FTRINIDI] ASC, [FTRSWVIS] ASC, [FTRSWVIN] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLFTR_7_1834541669__K1_K2_K52]
    ON [dbo].[METLFTR]([FTRINNOD] ASC, [FTRINIDI] ASC, [FTRSWVIN] ASC);


GO
CREATE STATISTICS [_dta_stat_1834541669_2_24_52_1]
    ON [dbo].[METLFTR]([FTRINIDI], [FTRSWVIS], [FTRSWVIN], [FTRINNOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç extern', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSEEX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Denominació del tràmit o gestió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció lliure de quip ot sol·lictiar el tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSQUI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Com es pot fer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSHOW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Com es pot fer presencialment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSPRS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Com es pot fer telefonicament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSTEL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'com es pot fer per internet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSWEB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Quan es pot demanar', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSQUA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions/altres informacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSOBS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Temps de resposta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSTRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Legislació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSLEG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text d’alerta. (per exemple: tràmit tancat, en procés d’inscripció..etc)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSALE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’inici de l’alerta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDTALI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de fi de l’alerta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDTALF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de publicació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDTPUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de caducitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDTCAD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRINUNI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tràmit a la carpeta ciutadana?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRSWCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No visible en seu electrònica però si en la resta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRSWVWE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'URL del formulari web dins de la carpeta ciutadana', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSFWE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRAD. Codi de tipus d''expedient', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRCDTTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRAD. Codi de tipus de tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRCDTTT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRAD. Codi del tipus de tràmit següent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRCDTTS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Trad. Identificador de la unitat organitzativa pel tràmit següent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRCDUTS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom curt', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSNOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Validesa del producte resultant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSVAL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requeriments per a atenció presencial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSRAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requeriments per a atenció telefònica', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSRAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requeriments per a atenció telemàtica', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSRAI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del flux global', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSDFG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció d''instruccions per a l''informador per a formalitzar-lo presencialment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSDIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció d''instruccions per a l''informador per a formalitzar-lo telefònicament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSDIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció d''instruccions per a l''informador per a formalitzar-lo telemàticament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRDSDII';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visualització de nivell de servei', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRWNNSV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRWNNS1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRWNNS2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTR', @level2type = N'COLUMN', @level2name = N'FTRWNNS3';

