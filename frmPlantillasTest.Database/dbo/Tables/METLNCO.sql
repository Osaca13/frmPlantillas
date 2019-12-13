CREATE TABLE [dbo].[METLNCO] (
    [NCOINNOD] INT            NOT NULL,
    [NCOINIDI] SMALLINT       NOT NULL,
    [NCODSTIT] VARCHAR (255)  NOT NULL,
    [NCODSUSR] CHAR (10)      NOT NULL,
    [NCODTANY] ROWVERSION     NOT NULL,
    [NCODSDES] VARCHAR (8000) NULL,
    [NCOSWCAP] CHAR (1)       CONSTRAINT [DF_METLNCO_NCOSWCAP] DEFAULT ('N') NOT NULL,
    [NCODSIMG] VARCHAR (100)  NULL,
    [NCODSIMO] VARCHAR (100)  NULL,
    [NCOSWCOD] CHAR (1)       CONSTRAINT [DF_METLNCO_NCOSWCOD] DEFAULT ('S') NOT NULL,
    CONSTRAINT [PK_METLNCO] PRIMARY KEY CLUSTERED ([NCOINNOD] ASC, [NCOINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLNCO_7_1316251794__K2_K1_3]
    ON [dbo].[METLNCO]([NCOINIDI] ASC, [NCOINNOD] ASC)
    INCLUDE([NCODSTIT]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del node codificació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNCO', @level2type = N'COLUMN', @level2name = N'NCODSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'és capa de cartografia?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNCO', @level2type = N'COLUMN', @level2name = N'NCOSWCAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Icona per represntar a guia urbana', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNCO', @level2type = N'COLUMN', @level2name = N'NCODSIMG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'node codificable. Si valor ''S'', es podran afegir continguts codificats des d''un formulari d''agenda/notícies/directori/tràmits..etc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNCO', @level2type = N'COLUMN', @level2name = N'NCOSWCOD';

