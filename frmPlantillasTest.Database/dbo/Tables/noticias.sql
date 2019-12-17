﻿CREATE TABLE [dbo].[noticias] (
    [id]        INT           NOT NULL,
    [canal]     INT           CONSTRAINT [DF__noticiasP__canal__4A44B052] DEFAULT ('0') NOT NULL,
    [fecha]     DATETIME      CONSTRAINT [DF__noticiasP__fecha__4B38D48B] DEFAULT ('0000-00-00 00:00:00') NOT NULL,
    [seccion]   INT           CONSTRAINT [DF__noticiasP__secci__4C2CF8C4] DEFAULT ('0') NOT NULL,
    [seccion2]  INT           CONSTRAINT [DF__noticiasP__secci__4D211CFD] DEFAULT ('0') NOT NULL,
    [seccion3]  INT           CONSTRAINT [DF__noticiasP__secci__4E154136] DEFAULT ('0') NOT NULL,
    [seccion4]  INT           CONSTRAINT [DF__noticiasP__secci__4F09656F] DEFAULT ('0') NOT NULL,
    [titulo]    VARCHAR (128) CONSTRAINT [DF__noticiasP__titul__4FFD89A8] DEFAULT ('') NOT NULL,
    [subtitulo] VARCHAR (200) CONSTRAINT [DF__noticiasP__subti__50F1ADE1] DEFAULT ('') NOT NULL,
    [intro]     VARCHAR (200) CONSTRAINT [DF__noticiasP__intro__51E5D21A] DEFAULT ('') NOT NULL,
    [cuerpo]    TEXT          NOT NULL,
    [recurso1]  VARCHAR (200) CONSTRAINT [DF__noticiasP__recur__52D9F653] DEFAULT ('') NOT NULL,
    [recurso2]  VARCHAR (200) CONSTRAINT [DF__noticiasP__recur__53CE1A8C] DEFAULT ('') NOT NULL,
    [recurso3]  VARCHAR (200) CONSTRAINT [DF__noticiasP__recur__54C23EC5] DEFAULT ('') NOT NULL,
    [recurso4]  VARCHAR (200) CONSTRAINT [DF__noticiasP__recur__55B662FE] DEFAULT ('') NOT NULL,
    [link1]     VARCHAR (200) CONSTRAINT [DF__noticiasP__link1__56AA8737] DEFAULT ('') NOT NULL,
    [link2]     VARCHAR (200) CONSTRAINT [DF__noticiasP__link2__579EAB70] DEFAULT ('') NOT NULL,
    [link3]     VARCHAR (200) CONSTRAINT [DF__noticiasP__link3__5892CFA9] DEFAULT ('') NOT NULL,
    [link4]     VARCHAR (200) CONSTRAINT [DF__noticiasP__link4__5986F3E2] DEFAULT ('') NOT NULL,
    [miniatura] VARCHAR (100) CONSTRAINT [DF__noticiasP__minia__5A7B181B] DEFAULT ('') NOT NULL,
    [foto]      VARCHAR (100) CONSTRAINT [DF__noticiasPr__foto__5B6F3C54] DEFAULT ('') NOT NULL,
    [estado]    CHAR (2)      CONSTRAINT [DF__noticiasP__estad__5C63608D] DEFAULT ('') NOT NULL,
    [autor]     VARCHAR (64)  CONSTRAINT [DF__noticiasP__autor__5D5784C6] DEFAULT ('') NOT NULL,
    [notas]     TEXT          NOT NULL,
    [visible]   CHAR (2)      CONSTRAINT [DF__noticiasP__visib__5E4BA8FF] DEFAULT ('') NOT NULL,
    [tipo]      INT           CONSTRAINT [DF__noticiasPr__tipo__5F3FCD38] DEFAULT ('0') NOT NULL
);
