CREATE TABLE [dbo].[METLLTR] (
    [LTRINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [LTRCDTRA] INT          CONSTRAINT [DF_METLLTR_LTRCDTRA] DEFAULT (0) NULL,
    [LTRCDPER] INT          CONSTRAINT [DF_METLLTR_LTRCDPER] DEFAULT (0) NOT NULL,
    [LTRDTFEC] DATETIME     NOT NULL,
    [LTRCDREG] INT          CONSTRAINT [DF_METLLTR_LTRCDREG] DEFAULT (0) NOT NULL,
    [LTRTPCAN] SMALLINT     CONSTRAINT [DF_METLLTR_LTRTPCAN] DEFAULT (0) NOT NULL,
    [LTRCDNIF] VARCHAR (10) CONSTRAINT [DF_METLLTR_LTRCDNIF] DEFAULT ('') NULL,
    CONSTRAINT [PK_METLLTR] PRIMARY KEY CLUSTERED ([LTRINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLLTR_7_786869920__K2_K1_4]
    ON [dbo].[METLLTR]([LTRCDTRA] ASC, [LTRINCOD] ASC)
    INCLUDE([LTRDTFEC]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[METLLTR]([LTRCDTRA] ASC, [LTRDTFEC] ASC)
    INCLUDE([LTRINCOD]);


GO
CREATE NONCLUSTERED INDEX [índex LTRCDNIF]
    ON [dbo].[METLLTR]([LTRCDNIF] ASC);


GO
CREATE NONCLUSTERED INDEX [METLLTR_LTRCDREG_LTRINCOD]
    ON [dbo].[METLLTR]([LTRCDREG] ASC)
    INCLUDE([LTRINCOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de log', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRCDTRA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi Persona que ha iniciat el tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRCDPER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d''inici de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRDTFEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de registre telemàtic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRCDREG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Canal d’entrada del tràmit.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRTPCAN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NIF de l''iniciador del tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLLTR', @level2type = N'COLUMN', @level2name = N'LTRCDNIF';

