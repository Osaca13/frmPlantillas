CREATE TABLE [dbo].[METLNDO] (
    [NDOINNOD] INT        CONSTRAINT [DF_METLNDO_NDOINNOD] DEFAULT ('') NOT NULL,
    [NDOINIDI] SMALLINT   NOT NULL,
    [NDODSTIT] TEXT       NOT NULL,
    [NDODTANY] ROWVERSION NOT NULL,
    [NDODSUSR] CHAR (10)  NOT NULL,
    [NDODTPUB] DATETIME   CONSTRAINT [DF_METLNDO_NDODTPUB] DEFAULT ('1 / 1 / 1900') NOT NULL,
    [NDODTCAD] DATETIME   CONSTRAINT [DF_METLNDO_NDODTCAD] DEFAULT ('1/1/2020') NOT NULL,
    [NDODSOBS] TEXT       NULL,
    CONSTRAINT [PK_METLNDO] PRIMARY KEY CLUSTERED ([NDOINNOD] ASC, [NDOINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNDO', @level2type = N'COLUMN', @level2name = N'NDOINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'observacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNDO', @level2type = N'COLUMN', @level2name = N'NDODSOBS';

