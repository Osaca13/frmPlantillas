CREATE TABLE [dbo].[CCITLDOC] (
    [DOCINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [DOCINPER] INT           NOT NULL,
    [DOCSTDES] VARCHAR (255) NOT NULL,
    [DOCDTCRE] DATETIME      CONSTRAINT [DF__CCITLDOC__DOCDTC__5105F123] DEFAULT (getdate()) NULL,
    [DOCDTLEC] DATETIME      NULL,
    [DOCSTURL] VARCHAR (255) NOT NULL,
    [DOCSTDAD] TEXT          NOT NULL,
    [DOCSTCLA] VARCHAR (50)  NOT NULL,
    [DOCBOELI] CHAR (1)      CONSTRAINT [DF_CCITLDOC_DOCBOELI] DEFAULT ('N') NOT NULL,
    [DOCDTVAL] DATETIME      NOT NULL,
    [DOCINREA] INT           NULL,
    [DOCINREG] INT           NULL,
    [DOCINREP] INT           NULL,
    CONSTRAINT [PK__CCITLDOC__5011CCEA] PRIMARY KEY CLUSTERED ([DOCINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK__CCITLDOC__DOCINP__51FA155C] FOREIGN KEY ([DOCINPER]) REFERENCES [dbo].[CCITLPER] ([PERINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registre E/S del Ajuntament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLDOC', @level2type = N'COLUMN', @level2name = N'DOCINREA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registre de GAIA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLDOC', @level2type = N'COLUMN', @level2name = N'DOCINREG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi usuari de la carpeta del representant', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLDOC', @level2type = N'COLUMN', @level2name = N'DOCINREP';

