CREATE TABLE [dbo].[METLNAS] (
    [NASINNOD] INT            NOT NULL,
    [NASINIDI] SMALLINT       NOT NULL,
    [NASDSTIT] VARCHAR (250)  NOT NULL,
    [NASDSTI2] VARCHAR (100)  NOT NULL,
    [NASDSUSR] CHAR (10)      NOT NULL,
    [NASDTANY] DATETIME       NOT NULL,
    [NASDSDES] VARCHAR (8000) NOT NULL,
    [NASDSPCL] VARCHAR (250)  NOT NULL,
    [NASDSOBS] VARCHAR (8000) NOT NULL,
    [NASSWVIS] CHAR (1)       CONSTRAINT [DF_METLNAS_NASSWVIS] DEFAULT ((0)) NOT NULL,
    [NASCDSIT] SMALLINT       CONSTRAINT [DF_METLNAS_NASCDSIT] DEFAULT ((1)) NOT NULL,
    [NASSWTIP] SMALLINT       CONSTRAINT [DF_METLNAS_NASSWTIP] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_METLNAS] PRIMARY KEY CLUSTERED ([NASINNOD] ASC, [NASINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció del node codificació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNAS', @level2type = N'COLUMN', @level2name = N'NASDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Intern/extern?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNAS', @level2type = N'COLUMN', @level2name = N'NASSWVIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estat (Esborrany/Publicació/Històric/Ocult)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNAS', @level2type = N'COLUMN', @level2name = N'NASCDSIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus: agrupació (0) o servei (1)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLNAS', @level2type = N'COLUMN', @level2name = N'NASSWTIP';

