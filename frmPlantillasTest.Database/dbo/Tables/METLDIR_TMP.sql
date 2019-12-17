﻿CREATE TABLE [dbo].[METLDIR_TMP] (
    [DIRINNOD]   DECIMAL (18)  NOT NULL,
    [DIRINIDI]   SMALLINT      NOT NULL,
    [DIRDSNOM]   VARCHAR (500) NOT NULL,
    [DIRDSSIG]   VARCHAR (50)  NULL,
    [DIRDSCIF]   VARCHAR (50)  NULL,
    [DIRDSACT]   TEXT          NULL,
    [DIRINCOL]   INT           NULL,
    [DIRINENT]   INT           NULL,
    [DIRIMPRE]   FLOAT (53)    NULL,
    [DIRDSHOR]   TEXT          NULL,
    [DIRDSTEL]   TEXT          NULL,
    [DIRDSOBS]   TEXT          NULL,
    [DIRCDCAR]   INT           NULL,
    [DIRDSCAR]   VARCHAR (250) NULL,
    [DIRDSNUM]   VARCHAR (50)  NULL,
    [DIRDSBIS]   VARCHAR (50)  NULL,
    [DIRDSESC]   VARCHAR (50)  NULL,
    [DIRDSPIS]   VARCHAR (50)  NULL,
    [DIRDSPOR]   VARCHAR (50)  NULL,
    [DIRCDBAR]   INT           NULL,
    [DIRDSBAR]   VARCHAR (250) NULL,
    [DIRCDDIS]   INT           NULL,
    [DIRDSDIS]   VARCHAR (250) NULL,
    [DIRDSCDP]   VARCHAR (5)   NULL,
    [DIRCDMUN]   INT           NULL,
    [DIRDSMUN]   VARCHAR (250) NULL,
    [DIRCDPRV]   INT           NULL,
    [DIRDSPRV]   VARCHAR (250) NULL,
    [DIRCDPAI]   INT           NULL,
    [DIRDSPAI]   VARCHAR (250) NULL,
    [DIRDTPUB]   DATETIME      NOT NULL,
    [DIRDTCAD]   DATETIME      NOT NULL,
    [DIRDTMAN]   DATETIME      NULL,
    [DIRDSPCL]   VARCHAR (500) NULL,
    [DIRWNCON]   BIGINT        NULL,
    [DIRDSCOM]   TEXT          NULL,
    [DIRCDAIDA1] TEXT          NOT NULL,
    [DIRCDAIDA2] TEXT          NOT NULL,
    [DIRWNTIP]   SMALLINT      NOT NULL,
    [DIRDTALT]   VARCHAR (50)  NULL,
    [DIRDTMOD]   BINARY (8)    NULL,
    [DIRDSWEB]   VARCHAR (255) NULL,
    [DIRDSEML]   VARCHAR (255) NULL,
    [DIRDTDIN]   VARCHAR (50)  NULL,
    [DIRDTDAC]   VARCHAR (50)  NULL,
    [DIRDSNRE]   SMALLINT      NULL,
    [DIRDTREG]   VARCHAR (50)  NULL,
    [DIRDSEST]   VARCHAR (20)  NULL,
    [DIRDSAMB]   CHAR (1)      NULL,
    [DIRDSSIT]   CHAR (6)      NULL,
    [DIRDSSOC]   SMALLINT      NULL,
    [DIRDSPRE]   INT           NULL,
    [DIRDSAPR]   SMALLINT      NULL,
    [DIRDSUSU]   VARCHAR (10)  NULL,
    [DIRSWVIS]   SMALLINT      NOT NULL,
    [DIRDSDES]   TEXT          NOT NULL,
    [DIRCDCOX]   INT           NULL,
    [DIRCDCOY]   INT           NULL
);
