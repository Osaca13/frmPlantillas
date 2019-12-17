﻿CREATE TABLE [CPDsa].[BUTLIGR] (
    [IGRCDNOD] INT           IDENTITY (1, 1) NOT NULL,
    [IGRCDPAL] VARCHAR (50)  NOT NULL,
    [IGRINREL] INT           NOT NULL,
    [IGRINIDI] SMALLINT      NOT NULL,
    [IGRCDTIP] VARCHAR (12)  NOT NULL,
    [IGRDSPOS] VARCHAR (300) NOT NULL,
    [IGRINNOD] INT           NOT NULL,
    [IGRDSPNT] FLOAT (53)    NOT NULL,
    [IGRWSPUB] BIT           NOT NULL,
    [IGRDHFEC] DATETIME      NOT NULL,
    [IGRDSPNO] FLOAT (53)    NOT NULL,
    [IGRSWTIT] BIT           NOT NULL,
    [IGRSWVIU] BIT           NOT NULL,
    [IGRDTPUB] SMALLDATETIME CONSTRAINT [DF_BUTLIGR_IGRDTPUB_1] DEFAULT ('1/1/2000') NULL
);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [CPDsa].[BUTLIGR]([IGRCDTIP] ASC, [IGRSWVIU] ASC, [IGRCDPAL] ASC)
    INCLUDE([IGRINREL], [IGRINIDI], [IGRINNOD], [IGRDSPNT], [IGRDHFEC]);


GO
CREATE NONCLUSTERED INDEX [BUTLIGR_DATA]
    ON [CPDsa].[BUTLIGR]([IGRCDTIP] ASC, [IGRWSPUB] ASC, [IGRSWVIU] ASC, [IGRCDPAL] ASC, [IGRDTPUB] ASC)
    INCLUDE([IGRINREL], [IGRINIDI], [IGRINNOD], [IGRDSPNT], [IGRDHFEC]);


GO
CREATE NONCLUSTERED INDEX [BUTLIGR_DATAPUB]
    ON [CPDsa].[BUTLIGR]([IGRWSPUB] ASC, [IGRSWVIU] ASC, [IGRCDPAL] ASC, [IGRDTPUB] ASC)
    INCLUDE([IGRINREL], [IGRINIDI], [IGRCDTIP], [IGRINNOD], [IGRDSPNT], [IGRDHFEC]);


GO
CREATE NONCLUSTERED INDEX [BUTLIGR_IGRINNOD_IGRCDPAL]
    ON [CPDsa].[BUTLIGR]([IGRINNOD] ASC, [IGRCDPAL] ASC)
    INCLUDE([IGRINREL], [IGRDSPNT]);


GO
CREATE NONCLUSTERED INDEX [BUTLIGR_IGRCDPAL_IGRINIDI_IGRCDTIP]
    ON [CPDsa].[BUTLIGR]([IGRCDPAL] ASC, [IGRINIDI] ASC, [IGRCDTIP] ASC)
    INCLUDE([IGRDSPOS], [IGRINNOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de visibilitat/publicació del contingut', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'BUTLIGR', @level2type = N'COLUMN', @level2name = N'IGRDTPUB';
