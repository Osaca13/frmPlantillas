CREATE TABLE [dbo].[INTLUSU] (
    [USUINUSU] INT            IDENTITY (1, 1) NOT NULL,
    [USUCDESC] INT            NOT NULL,
    [USUCDDNI] CHAR (9)       NOT NULL,
    [USUDSNOM] CHAR (50)      NOT NULL,
    [USUDSCG1] CHAR (50)      NOT NULL,
    [USUDSCG2] CHAR (50)      NOT NULL,
    [USUDSUSU] CHAR (8)       NOT NULL,
    [USUDSPWD] VARCHAR (5000) NOT NULL,
    [USUINPWD] INT            CONSTRAINT [DF_INTLUSU_USUINPWD] DEFAULT ((1)) NULL,
    [USUDSUSF] CHAR (8)       NOT NULL,
    [USUDSPWF] VARCHAR (5000) NOT NULL,
    [USUINPWF] INT            CONSTRAINT [DF_INTLUSU_USUINPWF] DEFAULT ((1)) NULL,
    [USUDSTLF] CHAR (9)       NULL,
    [USUDSEML] CHAR (55)      NULL,
    [USUDSCAR] CHAR (70)      NULL,
    [USUDHALT] SMALLDATETIME  NOT NULL,
    [USUDHMOD] SMALLDATETIME  NULL,
    [USUCDBLO] INT            NULL,
    [USUCDBAJ] INT            CONSTRAINT [DF_INTLUSU_USUCDBAJ] DEFAULT ((0)) NULL,
    [USUINTUS] INT            NULL,
    [USUDSPWS] CHAR (10)      NULL,
    CONSTRAINT [PK_INTLUSU] PRIMARY KEY CLUSTERED ([USUINUSU] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUINUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi escola/entitat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUCDESC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de l''usuari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Cognom', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSCG1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 Cognom', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSCG2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Usuari', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSUSU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Password', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSPWD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telèfon de contacte', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSTLF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e-mail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSEML';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cargo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDSCAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data d''alta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDHALT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data de modificació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUDHMOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero d''intents', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUCDBLO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de usuario 1- Escuela  2-Entidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLUSU', @level2type = N'COLUMN', @level2name = N'USUINTUS';

