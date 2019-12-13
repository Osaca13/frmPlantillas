CREATE TABLE [dbo].[METLENR] (
    [ENRINNOD] NUMERIC (18)  NOT NULL,
    [ENRDSDIP] VARCHAR (250) NOT NULL,
    [ENRDSVAL] TINYINT       NOT NULL,
    [ENRDTTIM] DATETIME      NULL,
    CONSTRAINT [PK_METLENR] PRIMARY KEY CLUSTERED ([ENRINNOD] ASC, [ENRDSDIP] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'timestamp de la votació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLENR', @level2type = N'COLUMN', @level2name = N'ENRDTTIM';

