CREATE TABLE [dbo].[JOVTLMOD] (
    [MODINCOD] INT          NOT NULL,
    [MODINIDI] SMALLINT     NOT NULL,
    [MODDSNOM] VARCHAR (80) NOT NULL,
    CONSTRAINT [PK_JOVTLMOD] PRIMARY KEY CLUSTERED ([MODINCOD] ASC, [MODINIDI] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_JOVTLMOD_METLIDI] FOREIGN KEY ([MODINIDI]) REFERENCES [dbo].[METLIDI] ([IDICDIDI])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom Modalitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLMOD', @level2type = N'COLUMN', @level2name = N'MODDSNOM';

