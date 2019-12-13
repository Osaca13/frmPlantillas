CREATE TABLE [dbo].[METLCSS2] (
    [CSSINTIP] SMALLINT      NOT NULL,
    [CSSINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [CSSDSTXT] VARCHAR (50)  CONSTRAINT [DF_METLCSS2_CSSDSTXT] DEFAULT ('') NOT NULL,
    [CSSDSTHO] INT           CONSTRAINT [DF_METLCSS2_CSSDSTHO] DEFAULT (0) NOT NULL,
    [CSSDSTHP] INT           CONSTRAINT [DF_METLCSS2_CSSDSTHP] DEFAULT (0) NOT NULL,
    [CSSDSCSS] VARCHAR (400) NULL,
    [CSSWNWIG] SMALLINT      CONSTRAINT [DF_METLCSS2_CSSWNNWI] DEFAULT (1) NULL,
    [CSSWNFLT] SMALLINT      NULL,
    CONSTRAINT [PK_METLCSS2] PRIMARY KEY CLUSTERED ([CSSINTIP] ASC, [CSSINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'width gaia, si =1posarà el width i el float automàticament des de GAIA en la cel·la afectada per l''estil', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCSS2', @level2type = N'COLUMN', @level2name = N'CSSWNWIG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'bool que indica si gaia ha de posar automàticament els float', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLCSS2', @level2type = N'COLUMN', @level2name = N'CSSWNFLT';

