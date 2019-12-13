CREATE TABLE [dbo].[METLHIT] (
    [HITINCOD] INT            IDENTITY (1, 1) NOT NULL,
    [HITDSPAG] VARCHAR (400)  NULL,
    [HITDTDAT] DATETIME       NULL,
    [HITDSPAR] VARCHAR (8000) NULL,
    [HITCDREL] INT            NULL,
    [HITCDNOD] INT            NULL,
    [HITWNROB] SMALLINT       CONSTRAINT [DF_METLHIT_HITWNROB] DEFAULT ((0)) NULL,
    [HITCDTIP] INT            CONSTRAINT [DF_METLHIT_HITCDTIP] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_LOGTLLOG] PRIMARY KEY CLUSTERED ([HITINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [METLHIT11]
    ON [dbo].[METLHIT]([HITDTDAT] ASC, [HITCDREL] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [Index_METLHIT_HITCDTIP]
    ON [dbo].[METLHIT]([HITCDTIP] ASC);


GO
CREATE NONCLUSTERED INDEX [METLHIT_METLREL_DATA]
    ON [dbo].[METLHIT]([HITWNROB] ASC, [HITCDTIP] ASC)
    INCLUDE([HITDTDAT], [HITCDREL]);


GO
CREATE NONCLUSTERED INDEX [METLHIT_METLREL_ROB_TIP]
    ON [dbo].[METLHIT]([HITCDREL] ASC, [HITWNROB] ASC, [HITCDTIP] ASC)
    INCLUDE([HITDTDAT]);


GO
CREATE NONCLUSTERED INDEX [METLHIT_HITWNROB_HITCDTIP_HITDTDAT, sysname,>]
    ON [dbo].[METLHIT]([HITWNROB] ASC, [HITCDTIP] ASC, [HITDTDAT] ASC)
    INCLUDE([HITDSPAR]);


GO
CREATE NONCLUSTERED INDEX [METLHIT_HITCDNOD_HITWNROB]
    ON [dbo].[METLHIT]([HITCDNOD] ASC, [HITWNROB] ASC)
    INCLUDE([HITDTDAT]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'identificador del contador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITINCOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'nom de la pàgina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITDSPAG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'data de l''accés a la pàgina', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITDTDAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'parametres de la crida', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITDSPAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 si l''accés s''ha fet mitjançant un bot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITWNROB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus de contingut que estem sumant

1- notícia
2- agenda
3- tràmit
4- cercador
5- espai estadístic
6-App
...', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLHIT', @level2type = N'COLUMN', @level2name = N'HITCDTIP';

