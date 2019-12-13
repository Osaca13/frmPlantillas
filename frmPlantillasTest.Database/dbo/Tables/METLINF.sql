CREATE TABLE [dbo].[METLINF] (
    [INFINNOD] NUMERIC (18)   NOT NULL,
    [INFINIDI] SMALLINT       NOT NULL,
    [INFDSTIT] VARCHAR (1000) NULL,
    [INFDSSUB] VARCHAR (1000) NULL,
    [INFDSRES] TEXT           NOT NULL,
    [INFDSTXT] TEXT           NOT NULL,
    [INFDTANY] SMALLDATETIME  NOT NULL,
    [INFDTPUB] SMALLDATETIME  NOT NULL,
    [INFDTCAD] SMALLDATETIME  NOT NULL,
    [INFDSUSR] CHAR (20)      NOT NULL,
    [INFDTTIM] CHAR (20)      NULL,
    [INFDSAVN] VARCHAR (255)  CONSTRAINT [DF__METLINF__INFDSAV__126A6B83] DEFAULT ('') NOT NULL,
    [INFDSLNK] VARCHAR (2000) NULL,
    [INFDSVID] VARCHAR (8000) NULL,
    [INFCDTIP] SMALLINT       CONSTRAINT [DF_METLINF_INFCDTIP] DEFAULT ((0)) NULL,
    [INFWNBAU] SMALLINT       CONSTRAINT [DF_METLINF_INFWNBAU] DEFAULT ((0)) NULL,
    [INFDSFON] TEXT           CONSTRAINT [DF_METLINF_INFDSFON] DEFAULT ('') NULL,
    [INFDSPCL] VARCHAR (250)  CONSTRAINT [DF_METLINF_INFDSPCL] DEFAULT ('') NULL,
    [INFDTPBK] DATETIME       CONSTRAINT [DF_METLINF_INFDTPBK] DEFAULT (((1)/(1))/(1900)) NULL,
    [INFDSPBK] TEXT           CONSTRAINT [DF_METLINF_INFDSPBK] DEFAULT ('') NULL,
    [INFWNREV] SMALLINT       CONSTRAINT [DF_METLINF_INFDSREV] DEFAULT ((0)) NULL,
    [INFWNOAC] CHAR (1)       CONSTRAINT [DF_METLINF_INFWNOAC] DEFAULT ('0') NULL,
    [INFWN010] CHAR (1)       CONSTRAINT [DF_METLINF_INFWN010] DEFAULT ('0') NULL,
    [INFDTMAN] SMALLDATETIME  NULL,
    [INFDSQDC] TEXT           NULL,
    CONSTRAINT [PK_METLINF] PRIMARY KEY CLUSTERED ([INFINNOD] ASC, [INFINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Enllaç extern de la informació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDSLNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'tipus de contingut al que fa referència la informació.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFCDTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'tipus d''informació: 0: informació, 1: Bitacola, 2:alerta, 3:última hora', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFWNBAU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Font', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDSFON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Paraules clau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDSPCL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'pendent de Backoffice. data i hora', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDTPBK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'pendent de Backoffice. Text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDSPBK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Periodicitat de la revisió en mesos 1:mensual, 3: trimestral, 6:semestral, 12: anual, 0 cap', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFWNREV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visible per a usuaris de OAC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFWNOAC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visible per a usuaris de 010', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFWN010';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Propera data de manteniment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLINF', @level2type = N'COLUMN', @level2name = N'INFDTMAN';

