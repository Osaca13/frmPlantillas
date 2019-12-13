CREATE TABLE [dbo].[SLTLFXE] (
    [FXEINFAR] BIGINT NOT NULL,
    [FXEINELE] INT    NOT NULL,
    [FXEWNEL]  INT    NOT NULL,
    CONSTRAINT [PK_SLTLFXE] PRIMARY KEY CLUSTERED ([FXEINFAR] ASC, [FXEINELE] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SLTLFXE_SLTLELE] FOREIGN KEY ([FXEINELE]) REFERENCES [dbo].[SLTLELE] ([ELCDCOD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Códi de la sol·licitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFXE', @level2type = N'COLUMN', @level2name = N'FXEINFAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi del element', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFXE', @level2type = N'COLUMN', @level2name = N'FXEINELE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Quantitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFXE', @level2type = N'COLUMN', @level2name = N'FXEWNEL';

