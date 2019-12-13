CREATE TABLE [dbo].[JOVTLAUT] (
    [AUTINCOD] INT           IDENTITY (1, 1) NOT NULL,
    [AUTDSNOM] VARCHAR (100) NOT NULL,
    [AUTDSUSU] VARCHAR (15)  NOT NULL,
    [AUTDSPWD] VARCHAR (44)  NOT NULL,
    [AUTDSFOT] VARCHAR (250) NULL,
    [AUTDTNAC] DATETIME      NULL,
    [AUTDSLNA] VARCHAR (100) NULL,
    [AUTDSADR] VARCHAR (100) NULL,
    [AUTDSTEL] VARCHAR (15)  NULL,
    [AUTDSFAX] VARCHAR (15)  NULL,
    [AUTDSEML] VARCHAR (50)  NULL,
    [AUTCDVAL] CHAR (1)      CONSTRAINT [DF_Table1_AUTCDVAL] DEFAULT ('N') NULL,
    [AUTCDERR] SMALLINT      CONSTRAINT [DF_Table1_AUTCDERR] DEFAULT (0) NULL,
    [AUTCDACC] DATETIME      NULL,
    [AUTDSNIF] VARCHAR (15)  NULL,
    [AUTWNADM] CHAR (1)      CONSTRAINT [DF_JOVTLAUT_AUTWNADM] DEFAULT ('N') NULL,
    CONSTRAINT [PK_Table1] PRIMARY KEY CLUSTERED ([AUTINCOD] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [User] UNIQUE NONCLUSTERED ([AUTDSUSU] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de l’autor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari per entrar a l’àrea privada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clau de hash en MD5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom del fitxer que representa la imatge que identifica a l’autor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSFOT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data naixement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDTNAC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lloc de naixement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSLNA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Autor Validat? (per defecte N), valors possibles (S,N)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTCDVAL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre d’intents d’accés erronis (per defecte =0)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTCDERR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de darrer accés', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTCDACC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Document d''identitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTDSNIF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Autor Administrador? (per defecte N), valors possibles (S,N)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JOVTLAUT', @level2type = N'COLUMN', @level2name = N'AUTWNADM';

