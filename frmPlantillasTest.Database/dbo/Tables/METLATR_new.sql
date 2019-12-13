CREATE TABLE [dbo].[METLATR_new] (
    [ATRCDLTR] INT      NOT NULL,
    [ATRINCOD] INT      NOT NULL,
    [ATRTPEST] SMALLINT CONSTRAINT [DF_METLATR_ATRTPEST_new] DEFAULT ((0)) NOT NULL,
    [ATRTPFEC] DATETIME NOT NULL,
    [ATRDSTXT] TEXT     CONSTRAINT [DF_METLATR_ATRDSTXT_new] DEFAULT ('') NOT NULL,
    [ATRDSXML] TEXT     NULL,
    CONSTRAINT [PK_METLATR_new] PRIMARY KEY CLUSTERED ([ATRCDLTR] ASC, [ATRINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLATR_METLLTR_new] FOREIGN KEY ([ATRCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLATR_7_1118015114__K1_K2_K3]
    ON [dbo].[METLATR_new]([ATRCDLTR] ASC, [ATRINCOD] ASC, [ATRTPEST] ASC);


GO
CREATE NONCLUSTERED INDEX [METLATR6]
    ON [dbo].[METLATR_new]([ATRCDLTR] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de log', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_new', @level2type = N'COLUMN', @level2name = N'ATRCDLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''acció sobre la petició', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_new', @level2type = N'COLUMN', @level2name = N'ATRINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''estat de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_new', @level2type = N'COLUMN', @level2name = N'ATRTPEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_new', @level2type = N'COLUMN', @level2name = N'ATRTPFEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Guardarem les dades particulars del tràmit en format xml', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_new', @level2type = N'COLUMN', @level2name = N'ATRDSXML';

