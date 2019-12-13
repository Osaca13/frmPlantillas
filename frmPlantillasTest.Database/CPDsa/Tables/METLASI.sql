CREATE TABLE [CPDsa].[METLASI] (
    [ASIINNOD] NUMERIC (18)  NOT NULL,
    [ASIDSUSR] VARCHAR (50)  NOT NULL,
    [ASIDSNOM] VARCHAR (150) NOT NULL,
    [ASIWNTIP] SMALLINT      CONSTRAINT [DF_METLASI_ASIWNTIP] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_METLASI_1] PRIMARY KEY CLUSTERED ([ASIINNOD] ASC, [ASIDSUSR] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de node d''agenda', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLASI', @level2type = N'COLUMN', @level2name = N'ASIINNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari de xarxa identificador de l''assistent a l''acte', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLASI', @level2type = N'COLUMN', @level2name = N'ASIDSUSR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom complert de l''assistent', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLASI', @level2type = N'COLUMN', @level2name = N'ASIDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus d''assistents. 0: opcional , 1: obligatori', @level0type = N'SCHEMA', @level0name = N'CPDsa', @level1type = N'TABLE', @level1name = N'METLASI', @level2type = N'COLUMN', @level2name = N'ASIWNTIP';

