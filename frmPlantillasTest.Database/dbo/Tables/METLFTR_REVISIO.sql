﻿CREATE TABLE [dbo].[METLFTR_REVISIO] (
    [FTRINNOD] DECIMAL (10)   NOT NULL,
    [FTRINIDI] SMALLINT       NOT NULL,
    [FTRDSNOM] VARCHAR (500)  NOT NULL,
    [FTRDSEEX] TEXT           NULL,
    [FTRDSDES] TEXT           NULL,
    [FTRDSQUI] TEXT           NULL,
    [FTRDSHOW] TEXT           NULL,
    [FTRDSPRS] TEXT           NULL,
    [FTRDSTEL] TEXT           NULL,
    [FTRDSWEB] TEXT           NULL,
    [FTRDSQUA] VARCHAR (8000) NULL,
    [FTRDSOBS] TEXT           NULL,
    [FTRDSPRE] TEXT           NULL,
    [FTRDSTRE] TEXT           NULL,
    [FTRDSLEG] TEXT           NULL,
    [FTRDSALE] VARCHAR (80)   NULL,
    [FTRDTALI] DATETIME       NULL,
    [FTRDTALF] DATETIME       NOT NULL,
    [FTRDTPUB] DATETIME       NULL,
    [FTRDTCAD] DATETIME       NULL,
    [FTRDTMAN] DATETIME       NULL,
    [FTRDSPCL] VARCHAR (500)  NULL,
    [FTRDSCOM] TEXT           NULL,
    [FTRSWVIS] CHAR (1)       NULL,
    [FTRDSCOR] VARCHAR (150)  NULL,
    [FTRINUNI] INT            NULL,
    [FTRSWCAR] CHAR (1)       NULL,
    [FTRCDTRA] SMALLINT       NULL,
    [FTRCDRTE] NVARCHAR (50)  NULL,
    [FTRDSFAJ] VARCHAR (8000) NULL,
    [FTRDSFDO] VARCHAR (8000) NULL,
    [FTRSWAOC] CHAR (1)       NULL,
    [FTRCDSIL] CHAR (1)       NULL,
    [FTRDSDEC] VARCHAR (8000) NULL,
    [FTRCDORG] INT            NULL,
    [FTRSWNOT] CHAR (2)       NULL,
    [FTRSWTPV] CHAR (2)       NULL,
    [FTRSWTEM] CHAR (2)       NULL,
    [FTRSWREG] CHAR (2)       NULL,
    [FTRSWIDN] CHAR (2)       NULL,
    [FTRDSTEC] TEXT           NULL,
    [FTRDSOBJ] TEXT           NULL,
    [FTRDSETP] TEXT           NULL,
    [FTRDSIND] TEXT           NULL,
    [FTRDSTER] TEXT           NULL,
    [FTRDSTDO] TEXT           NULL,
    [FTRDSPDO] TEXT           NULL,
    [FTRSWVUD] CHAR (2)       NULL,
    [FTRDSQDC] TEXT           NULL,
    [FTRDSAPD] TEXT           NULL,
    [FTRSWVSE] CHAR (1)       NULL,
    [FTRSWVIN] CHAR (1)       NULL,
    [FTRSWVWE] CHAR (1)       NULL,
    [FTRDSFWE] VARCHAR (100)  NULL,
    CONSTRAINT [PK_METLFTR_REVISIO] PRIMARY KEY CLUSTERED ([FTRINNOD] ASC, [FTRINIDI] ASC) WITH (FILLFACTOR = 90)
);
