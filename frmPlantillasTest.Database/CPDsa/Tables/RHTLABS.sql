CREATE TABLE [CPDsa].[RHTLABS] (
    [ABSCDLTR] INT           NOT NULL,
    [ABSDSNIF] CHAR (21)     NULL,
    [ABSINPER] INT           NULL,
    [ABSDSNOM] VARCHAR (150) NULL,
    [ABSDSDEP] VARCHAR (500) NULL,
    [ABSDSRSI] VARCHAR (50)  NULL,
    [ABSDSCON] VARCHAR (50)  NULL,
    [ABSSWHCP] CHAR (1)      NULL,
    [ABSDSMOT] TEXT          NULL,
    [ABSDTHSO] CHAR (5)      NULL,
    [ABSDTHRE] CHAR (5)      NULL,
    [ABSDTTOT] CHAR (5)      NULL,
    [ABSDTDTC] DATETIME      NULL,
    [ABSCDMAR] CHAR (3)      NULL,
    [ABSDSMAR] VARCHAR (100) NULL,
    [ABSDTDAB] DATETIME      NULL,
    [ABSDTDFI] DATETIME      NULL,
    [ABSDSNCP] VARCHAR (150) NULL,
    [ABSDSECP] VARCHAR (50)  NULL,
    [ABSDSNRH] VARCHAR (150) NULL,
    [ABSDSERH] VARCHAR (50)  NULL,
    [ABSDSNRS] VARCHAR (150) NULL,
    [ABSDSERS] VARCHAR (50)  NULL,
    [ABSDSTIP] CHAR (1)      NULL,
    [ABSDSDOC] TEXT          NULL,
    [ABSDTDAL] VARCHAR (500) NULL,
    [ABSDSRES] CHAR (1)      NULL,
    CONSTRAINT [PK_RHTLABS] PRIMARY KEY CLUSTERED ([ABSCDLTR] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ABSDSECP]
    ON [CPDsa].[RHTLABS]([ABSDSECP] ASC);


GO
CREATE NONCLUSTERED INDEX [ABSDSERS]
    ON [CPDsa].[RHTLABS]([ABSDSERS] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RHTLABS]
    ON [CPDsa].[RHTLABS]([ABSDSECP] ASC, [ABSDSTIP] ASC, [ABSDSERS] ASC);


GO
CREATE NONCLUSTERED INDEX [ABSDSTIP]
    ON [CPDsa].[RHTLABS]([ABSDSTIP] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de taula METLLTR', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSCDLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NIF', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSNIF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de PERSONAL', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSINPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom i cognoms', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ponència,Departament', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSDEP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Representació sindical', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSRSI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Condició', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSCON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hores computables o no ', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSSWHCP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Motiu de  l''absència', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSMOT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora de sortida', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTHSO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora de retorno', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTHRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora Total', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTTOT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de la comunicació', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTDTC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi marcatge', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSCDMAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del marcatge', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSMAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de l''absència Ini', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTDAB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de l''absencia Fi', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTDFI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del cap del soli·licitant', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSNCP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu del Cap', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSECP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del responsable de RRHH', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSNRH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu del responsable de RRHH', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSERH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del representante sindical (S) o solicitante (G) ', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSNRS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Correu del representante sindical (S) o del solicitante(G)', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSERS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de absència (Sindical (S), General (G))', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Documentos separados por |', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSDOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'String con días alternos separados por comas', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDTDAL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reservar dia (''S'',''N'')', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'RHTLABS', @level2type = N'COLUMN', @level2name = N'ABSDSRES';

