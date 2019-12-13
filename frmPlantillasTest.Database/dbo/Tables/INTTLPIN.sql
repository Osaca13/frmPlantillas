CREATE TABLE [dbo].[INTTLPIN] (
    [INTINNIF] CHAR (10) NOT NULL,
    [INTCDPIN] CHAR (4)  NOT NULL,
    [INTDTMOD] DATETIME  NULL,
    [INTDTACC] DATETIME  NULL,
    [INTCDBLO] INT       CONSTRAINT [DF_INTTLPIN_INTCDBLO] DEFAULT (0) NOT NULL,
    [INTDSADR] CHAR (20) NULL,
    CONSTRAINT [PK_INTTLPIN] PRIMARY KEY NONCLUSTERED ([INTINNIF] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adreça IP de la connexió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTTLPIN', @level2type = N'COLUMN', @level2name = N'INTDSADR';

