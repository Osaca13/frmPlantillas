CREATE TABLE [dbo].[SLTLFAR] (
    [FAINLTR]  INT      NOT NULL,
    [FADHDATA] DATETIME NOT NULL,
    [FAWNTRE]  INT      NULL,
    [FATPFAR]  CHAR (1) NULL,
    [FASWREP]  CHAR (1) NULL,
    CONSTRAINT [PK_SLTLFAR] PRIMARY KEY CLUSTERED ([FAINLTR] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de trámite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFAR', @level2type = N'COLUMN', @level2name = N'FAINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de la petició', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFAR', @level2type = N'COLUMN', @level2name = N'FADHDATA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de treballadors que presta servei aquesta farmaciola', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFAR', @level2type = N'COLUMN', @level2name = N'FAWNTRE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Farmaciola complerta(C) vehicle(V)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFAR', @level2type = N'COLUMN', @level2name = N'FATPFAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reposició (S/N)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SLTLFAR', @level2type = N'COLUMN', @level2name = N'FASWREP';

