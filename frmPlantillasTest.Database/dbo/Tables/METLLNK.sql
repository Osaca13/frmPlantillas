CREATE TABLE [dbo].[METLLNK] (
    [LNKINNOD] INT            NOT NULL,
    [LNKINIDI] SMALLINT       NOT NULL,
    [LNKDSTXT] VARCHAR (8000) NOT NULL,
    [LNKDSLNK] VARCHAR (8000) NOT NULL,
    [LNKWNTIP] SMALLINT       NOT NULL,
    [LNKDTPUB] SMALLDATETIME  NOT NULL,
    [LNKDTCAD] SMALLDATETIME  NULL,
    [LNKDSUSR] CHAR (10)      NOT NULL,
    [LNKDTTIM] BINARY (8)     NULL,
    [LNKCDREL] DECIMAL (18)   NULL,
    [LNKDSDES] VARCHAR (8000) NULL,
    [LNKDSVID] VARCHAR (8000) NULL
);


GO
CREATE NONCLUSTERED INDEX [METLLNK_LNKINNOD,LNKINIDI]
    ON [dbo].[METLLNK]([LNKINNOD] ASC, [LNKINIDI] ASC)
    INCLUDE([LNKDSTXT], [LNKDSLNK], [LNKCDREL]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1. Enlace se abre en una nueva ventana. 2. No se abre en nueva ventana', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLNK', @level2type = N'COLUMN', @level2name = N'LNKWNTIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'(''Descripció de l''''enllaç'')', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLNK', @level2type = N'COLUMN', @level2name = N'LNKDSDES';

