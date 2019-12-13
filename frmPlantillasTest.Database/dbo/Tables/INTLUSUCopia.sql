﻿CREATE TABLE [dbo].[INTLUSUCopia] (
    [USUINUSU] INT            NOT NULL,
    [USUCDESC] INT            NOT NULL,
    [USUCDDNI] CHAR (9)       NOT NULL,
    [USUDSNOM] CHAR (50)      NOT NULL,
    [USUDSCG1] CHAR (50)      NOT NULL,
    [USUDSCG2] CHAR (50)      NOT NULL,
    [USUDSUSU] CHAR (8)       NOT NULL,
    [USUDSPWD] VARCHAR (5000) NOT NULL,
    [USUINPWD] INT            NULL,
    [USUDSUSF] CHAR (8)       NOT NULL,
    [USUDSPWF] VARCHAR (5000) NOT NULL,
    [USUINPWF] INT            NULL,
    [USUDSTLF] CHAR (9)       NULL,
    [USUDSEML] CHAR (50)      NULL,
    [USUDSCAR] CHAR (70)      NULL,
    [USUDHALT] SMALLDATETIME  NOT NULL,
    [USUDHMOD] SMALLDATETIME  NULL,
    [USUCDBLO] INT            NULL,
    [USUCDBAJ] INT            NULL,
    [USUINTUS] INT            NULL
);

