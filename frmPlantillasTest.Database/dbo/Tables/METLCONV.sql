﻿CREATE TABLE [dbo].[METLCONV] (
    [CONINVER] INT            IDENTITY (1, 1) NOT NULL,
    [CONINNOD] NUMERIC (18)   NOT NULL,
    [CONINIDI] SMALLINT       NOT NULL,
    [CONDSDES] TEXT           NOT NULL,
    [CONDTPBC] TEXT           NULL,
    [CONDSPRE] TEXT           NOT NULL,
    [CONDTFIN] TEXT           NOT NULL,
    [CONDSIMG] CHAR (1)       NULL,
    [CONDTPUB] DATETIME       NOT NULL,
    [CONDTCAD] DATETIME       NULL,
    [CONDSUSR] CHAR (10)      NOT NULL,
    [CONDTTIM] DATETIME       NULL,
    [CONDSINC] VARCHAR (2000) CONSTRAINT [DF_METLCONV_CONDSINC] DEFAULT ('') NOT NULL,
    [CONDSPRO] VARCHAR (2000) CONSTRAINT [DF_METLCONV_CONDSPRO] DEFAULT ('') NOT NULL,
    [CONDSEXP] VARCHAR (50)   CONSTRAINT [DF_METLCONV_CONDSEXP] DEFAULT ('') NULL,
    [CONSWANU] CHAR (1)       CONSTRAINT [DF_METLCONV_CONSWANU] DEFAULT ('N') NULL,
    [CONSWSUS] CHAR (1)       CONSTRAINT [DF_METLCONV_CONSWSUS] DEFAULT ('N') NULL,
    [CONDSTLI] TEXT           NULL,
    [CONDSPSE] VARCHAR (100)  NULL,
    [CONDSCSE] VARCHAR (100)  NULL,
    [CONDTBOP] DATETIME       NULL,
    [CONDTDOG] DATETIME       NULL,
    [CONDTBOE] DATETIME       NULL,
    [CONDTDOE] DATETIME       NULL,
    [CONDSDLI] VARCHAR (100)  NULL,
    [CONDSOPR] TEXT           NULL,
    [CONDSADP] VARCHAR (100)  NULL,
    [CONDSAPC] TEXT           NULL,
    [CONDSAPI] VARCHAR (100)  NULL,
    [CONDSADF] VARCHAR (100)  NULL,
    [CONDSAFC] TEXT           NULL,
    [CONDSAFI] VARCHAR (100)  NULL,
    [CONDSPEC] VARCHAR (100)  NULL,
    [CONDTAPD] DATETIME       NULL,
    [CONSWFUT] CHAR (1)       CONSTRAINT [DF_METLCONV_CONSWFUT] DEFAULT ('N') NULL,
    [CONSWCOP] CHAR (1)       CONSTRAINT [DF_METLCONV_CONSWCOP] DEFAULT ('N') NULL,
    [CONDTFOR] DATETIME       NULL,
    [CONDTAPC] DATE           NULL,
    [CONSWRES] CHAR (1)       CONSTRAINT [DF_METLCONV_CONSWRES] DEFAULT ('N') NULL,
    [CONDSCON] TEXT           NULL,
    CONSTRAINT [PK_METLCONV] PRIMARY KEY CLUSTERED ([CONINVER] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de versió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONINVER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la incidència', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSINC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del procediment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSPRO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número d''expedient', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSEXP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contracte anul·lat (o modificat)?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONSWANU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contracte suspès?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONSWSUS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data límit de presentació d’ofertes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSTLI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Procediment de selecció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSPSE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Criteris de selecció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSCSE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data publicació en BOP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTBOP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data publicació en DOG', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTDOG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data publicació en BOE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTBOE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data publicació en DOUE (unió europea)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTDOE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data límit de presentació d''ofertes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSDLI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Obertura proposicions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSOPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’adjudicació provisional', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSADP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contractista adjudicació provisional', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSAPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d’adjudicació definitiva', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSADF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contractista adjudicació definitiva', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSAFC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Import contractació definitiva', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSAFI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Perfil del contractant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSPEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de Publicació de l’adjudicació provisional', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTAPD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'S: Contracte futur?
N: Contracte actual (valor per defecte)
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONSWFUT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Còpies útils a copisteria?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONSWCOP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Anunci d’informació pública. Plec de clàusules', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDTAPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'S: Contracte reservat
N: Contracte no reservat (valor per defecte)
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONSWRES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Condicions de solvència exigibles', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCONV', @level2type = N'COLUMN', @level2name = N'CONDSCON';

