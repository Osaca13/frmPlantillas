CREATE TABLE [dbo].[METLAPP] (
    [APPINCOD] INT            IDENTITY (1, 1) NOT NULL,
    [APPDSNOM] VARCHAR (250)  CONSTRAINT [DF_METLAPP_APPDSNOM] DEFAULT ('') NOT NULL,
    [APPDSDES] VARCHAR (1000) CONSTRAINT [DF_METLAPP_APPDSDES] DEFAULT ('') NOT NULL,
    [APPDSDE2] VARCHAR (1000) CONSTRAINT [DF_METLAPP_APPDSDE2] DEFAULT ('') NOT NULL,
    [APPDSDE3] VARCHAR (1000) CONSTRAINT [DF_METLAPP_APPDSDE3] DEFAULT ('') NOT NULL,
    [APPDSAND] VARCHAR (100)  CONSTRAINT [DF_METLAPP_APPDSAND] DEFAULT ('') NOT NULL,
    [APPDSIOS] VARCHAR (100)  CONSTRAINT [DF_METLAPP_APPDSIOS] DEFAULT ('') NOT NULL,
    [APPDSWIN] VARCHAR (1000) CONSTRAINT [DF_METLAPP_APPDSWIN] DEFAULT ('') NOT NULL,
    [APPDSURA] VARCHAR (250)  CONSTRAINT [DF_METLAPP_APPDSURA] DEFAULT ('') NOT NULL,
    [APPDSUIO] VARCHAR (250)  CONSTRAINT [DF_METLAPP_APPDSUIO] DEFAULT ('') NOT NULL,
    [APPDSUWI] VARCHAR (250)  CONSTRAINT [DF_METLAPP_APPDSUWI] DEFAULT ('') NOT NULL,
    [APPCDPAR] INT            NOT NULL,
    [APPDSICO] VARCHAR (250)  NULL,
    [APPDSIC2] VARCHAR (250)  CONSTRAINT [DF_METLAPP_APPDSIC2] DEFAULT ('') NOT NULL,
    [APPWNORD] SMALLINT       NULL,
    CONSTRAINT [PK_METLAPP] PRIMARY KEY CLUSTERED ([APPINCOD] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador d''aplicació, autonuméric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nom de l''aplicació que apareixerà a les stores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSNOM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l''aplicació en català', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSDES';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l''aplicació en castellà', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSDE2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de l''aplicació en anglès', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSDE3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador necesari per llançar l''app en Android', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSAND';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador necesari, en IOS, per iniciar l''aplicació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSIOS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador necesari, en Windows mobile, per iniciar l''aplicació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSWIN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'URL per poder instal·lar l''app en Android si no està instal·lada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSURA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'URL per poder instal·lar l''app en IOS si no està instal·lada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSUIO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'URL per poder instal·lar l''app en Windows mobile  si no està instal·lada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSUWI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi d''aplicació pare, la que conté aplicacions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPCDPAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Url de la icona de l''app mida petita', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSICO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Url de la Icona de l''app, mida gran', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLAPP', @level2type = N'COLUMN', @level2name = N'APPDSIC2';

