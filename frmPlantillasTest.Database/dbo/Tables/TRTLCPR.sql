CREATE TABLE [dbo].[TRTLCPR] (
    [CPRINCOD] INT      NOT NULL,
    [CPRWNTAC] SMALLINT NOT NULL,
    [CPRDTDAT] DATETIME NOT NULL,
    [CPRDTDPE] DATETIME NULL,
    [CPRCDMAR] SMALLINT NOT NULL,
    [CPRCDNMA] SMALLINT NULL,
    [CPRWNDES] TEXT     NOT NULL,
    CONSTRAINT [PK_TRTLCPR] PRIMARY KEY CLUSTERED ([CPRINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de tràmit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus Acció:1: nou marcatge 2: modificar marcatge 3: esborrar marcatge 4:altres', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRWNTAC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data de la petició', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRDTDAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data del marcatge a tractar (null en cas d’acció: “altres”)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRDTDPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi tipus de marcatge actual (null en cas d’acció : “afegir marcatge” o “altres”)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRCDMAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nou Marcatge: codi del nou marcatge( null en cas d’acció: “esborrar marcatge” o “altres”)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRCDNMA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Motiu de la petició. En cas d’una acció de tipus “altres” en aquest camp tindrem la descripció de la petició. És obligatori introduir-ho.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLCPR', @level2type = N'COLUMN', @level2name = N'CPRWNDES';

