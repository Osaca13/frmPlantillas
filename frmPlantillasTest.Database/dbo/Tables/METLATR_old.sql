CREATE TABLE [dbo].[METLATR_old] (
    [ATRCDLTR] INT      NOT NULL,
    [ATRINCOD] INT      NOT NULL,
    [ATRTPEST] SMALLINT CONSTRAINT [DF_METLATR_ATRTPEST] DEFAULT (0) NOT NULL,
    [ATRTPFEC] DATETIME NOT NULL,
    [ATRDSTXT] TEXT     CONSTRAINT [DF_METLATR_ATRDSTXT] DEFAULT ('') NOT NULL,
    [ATRDSXML] TEXT     NULL,
    CONSTRAINT [PK_METLATR] PRIMARY KEY CLUSTERED ([ATRCDLTR] ASC, [ATRINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_METLATR_METLLTR] FOREIGN KEY ([ATRCDLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [METLATR6]
    ON [dbo].[METLATR_old]([ATRCDLTR] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [_dta_index_METLATR_7_1118015114__K1_K2_K3]
    ON [dbo].[METLATR_old]([ATRCDLTR] ASC, [ATRINCOD] ASC, [ATRTPEST] ASC);


GO
CREATE STATISTICS [_dta_stat_1118015114_3_2]
    ON [dbo].[METLATR_old]([ATRTPEST], [ATRINCOD]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de log', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_old', @level2type = N'COLUMN', @level2name = N'ATRCDLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''acció sobre la petició', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_old', @level2type = N'COLUMN', @level2name = N'ATRINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi d''estat de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_old', @level2type = N'COLUMN', @level2name = N'ATRTPEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de l''acció', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_old', @level2type = N'COLUMN', @level2name = N'ATRTPFEC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Guardarem les dades particulars del tràmit en format xml', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLATR_old', @level2type = N'COLUMN', @level2name = N'ATRDSXML';

