CREATE TABLE [dbo].[METLMAI] (
    [MAIINCOD] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MAICDREL] NUMERIC (18) NOT NULL,
    [MAIDTDAT] DATETIME     NOT NULL,
    [MAICDNOD] NUMERIC (18) NOT NULL,
    [MAIDTTEM] INT          NOT NULL,
    CONSTRAINT [PK_METLMAI] PRIMARY KEY CLUSTERED ([MAIINCOD] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi autonumèric d''entrada a la cua de publicació de documents pdf per imprimir', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAI', @level2type = N'COLUMN', @level2name = N'MAIINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de relació del contingut que volem generar en PDF', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAI', @level2type = N'COLUMN', @level2name = N'MAICDREL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d''entrada en cua', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAI', @level2type = N'COLUMN', @level2name = N'MAIDTDAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Node del contingut que volem generar en pdf', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAI', @level2type = N'COLUMN', @level2name = N'MAICDNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Temps necessari per publicar el contingut', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLMAI', @level2type = N'COLUMN', @level2name = N'MAIDTTEM';

