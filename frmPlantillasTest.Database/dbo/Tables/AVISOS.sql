﻿CREATE TABLE [dbo].[AVISOS] (
    [NRO]          SMALLINT  IDENTITY (1, 1) NOT NULL,
    [FECHAALTA]    DATETIME  NOT NULL,
    [FECHACAD]     DATETIME  NOT NULL,
    [DEPARTAMENTO] CHAR (25) NULL,
    [TEXTO]        TEXT      NOT NULL,
    [LINK]         TEXT      NULL,
    [IMATGE]       CHAR (30) NULL
);

