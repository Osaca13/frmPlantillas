CREATE TABLE [dbo].[METLMPA] (
    [MPAINMPA] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MPACDREL] NUMERIC (18) CONSTRAINT [DF_METLMPA_MPACDREL] DEFAULT ((0)) NULL,
    [MPACDIDI] SMALLINT     NOT NULL,
    [MPADTPUB] DATETIME     NOT NULL,
    [MPADTCAD] DATETIME     NOT NULL,
    [MPACDNOD] NUMERIC (18) CONSTRAINT [DF_METLPA_MPACDNOD] DEFAULT ((0)) NULL,
    [MPACDREO] NUMERIC (18) NULL,
    [MPAWNASC] SMALLINT     NULL,
    [MPAWNEST] SMALLINT     NULL,
    [MPADTTEM] INT          CONSTRAINT [DF_METLMPA_MPADTTEM2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_METLMPA_1] PRIMARY KEY CLUSTERED ([MPAINMPA] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi relació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPACDREL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi Idioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPACDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de publicació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPADTPUB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de caducitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPADTCAD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi Node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPACDNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de relació original', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPACDREO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'manteniment d''ascendents? 1: si, 0:no', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPAWNASC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'manteniment només de la cel·la on intervé el contingut, 0: no, 1: si', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPAWNEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Temps utilitzat per fer el tractament', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMPA', @level2type = N'COLUMN', @level2name = N'MPADTTEM';

