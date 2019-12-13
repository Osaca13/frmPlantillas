CREATE TABLE [CPDsa].[LDTLAUT] (
    [AUTINLLD] INT          NOT NULL,
    [AUTDSUSU] VARCHAR (20) NULL,
    CONSTRAINT [FK_LDTLAUT_LDTLLLD] FOREIGN KEY ([AUTINLLD]) REFERENCES [CPDsa].[LDTLLLD] ([LLDINLLD])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id. Llista distribució', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'LDTLAUT', @level2type = N'COLUMN', @level2name = N'AUTINLLD';

