CREATE TABLE [dbo].[INTLACT] (
    [ACTINACT] INT             IDENTITY (1, 1) NOT NULL,
    [ACTDHANY] INT             NOT NULL,
    [ACTDHINI] SMALLDATETIME   NOT NULL,
    [ACTDSTIT] NVARCHAR (1000) NOT NULL,
    [ACTDSDES] TEXT            NULL,
    [ACTDSAFO] INT             NOT NULL,
    [ACTDSMIN] INT             CONSTRAINT [DF_INTLACT_ACTDSMIN] DEFAULT (0) NOT NULL,
    [ACTDHDUH] SMALLINT        NOT NULL,
    [ACTDHDUM] SMALLINT        NOT NULL,
    [ACTWNPPU] FLOAT (53)      NOT NULL,
    [ACTDHFIN] SMALLDATETIME   NOT NULL,
    [ACTDSANU] CHAR (10)       NOT NULL,
    [ACTWNPCO] FLOAT (53)      NULL,
    [ACTWNPPR] FLOAT (53)      NULL,
    [ACTCDENT] INT             CONSTRAINT [DF_INTLACT_ACTCDENT] DEFAULT (0) NOT NULL,
    [ACTDSPDF] NVARCHAR (100)  NULL,
    [ACTTPRES] CHAR (10)       CONSTRAINT [DF_INTLACT_ACTTPRES] DEFAULT ('') NOT NULL,
    [ACTDSOBS] TEXT            NULL,
    [ACTCDACU] INT             NOT NULL,
    [ACTDSOBJ] TEXT            NULL,
    [ACTINCPR] INT             NULL,
    [ACTDSCPR] TEXT            NULL,
    [ACTINMON] INT             NULL,
    [ACTDSMON] TEXT            NULL,
    [ACTINMSO] INT             NULL,
    [ACTDSMSO] TEXT            NULL,
    [ACTDSNLR] NVARCHAR (100)  NULL,
    [ACTDSLLR] NVARCHAR (100)  NULL,
    [ACTDSPLR] NVARCHAR (100)  NULL,
    [ACTDSCAR] TEXT            NULL,
    [ACTDSCOC] TEXT            NULL,
    [ACTDSCOP] TEXT            NULL,
    [ACTDSCOA] TEXT            NULL,
    [ACTINVNE] INT             NULL,
    [ACTDSVNE] TEXT            NULL,
    [ACTINRED] INT             NULL,
    [ACTINRCC] INT             NULL,
    [ACTINAMI] INT             NULL,
    [ACTDSMOD] VARCHAR (500)   NULL,
    [ACTINPPG] FLOAT (53)      NULL,
    [ACTDSIMG] TEXT            NULL,
    CONSTRAINT [PK_INTLACT] PRIMARY KEY CLUSTERED ([ACTINACT] ASC, [ACTDHANY] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi activitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Any del curs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDHANY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data inici', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDHINI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Titul', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSTIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Aforo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSAFO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Plazas minusvalidos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSMIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Duració .Hores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDHDUH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Duració. Minuts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDHDUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Precio escola publica(el válido)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTWNPPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de finalització', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDHFIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Anulado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSANU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Preu concertada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTWNPCO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Preu escola Privada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTWNPPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Entitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTCDENT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció PDF', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSPDF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reserva on-line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTTPRES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSOBS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Objetius', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSOBJ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'check. conocimientos previos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINCPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Conocimientos previos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSCPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Check. Monotirizaje', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINMON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Monotirizaje', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSMON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Check. Material de soporte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINMSO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Material de soporte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSMSO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom Lloc on es realitza', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSNLR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça on es realitza', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSLLR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Poblacio on es realitza', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSPLR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Check. Vinculacio no escolars', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINVNE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Vinculacio no escolars', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSVNE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Check. Revisado educación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINRED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Check. Revisado Corrector', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTINRCC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campos modificados', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSMOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Imatge', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLACT', @level2type = N'COLUMN', @level2name = N'ACTDSIMG';

