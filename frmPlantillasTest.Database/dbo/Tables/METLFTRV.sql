CREATE TABLE [dbo].[METLFTRV] (
    [FTRINNOD] DECIMAL (10)   NOT NULL,
    [FTRINIDI] SMALLINT       NOT NULL,
    [FTRINVER] SMALLINT       NOT NULL,
    [FTRCDFOE] INT            NOT NULL,
    [FTRCDFOR] INT            NOT NULL,
    [FTRDTDAT] DATETIME       NOT NULL,
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
    [FTRDTALF] DATETIME       NULL,
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
    [FTRDSPDO] TEXT           NULL,
    [FTRDSTDO] TEXT           NULL,
    [FTRDSAPD] TEXT           NULL,
    [FTRSWVUD] CHAR (2)       NULL,
    [FTRDSQDC] TEXT           NULL,
    [FTRSWVSE] CHAR (1)       CONSTRAINT [DF_METLFTRV_FTRSWVSE] DEFAULT ('0') NULL,
    [FTRSWVIN] CHAR (1)       CONSTRAINT [DF_METLFTRV_FTRSWVIN] DEFAULT ('0') NULL,
    [FTRSWVWE] CHAR (1)       CONSTRAINT [DF_METLFTRV_FTRSWVEW] DEFAULT ('0') NULL,
    [FTRDSFWE] VARCHAR (100)  NULL,
    [FTRCDTTE] VARCHAR (20)   CONSTRAINT [DF_METLFTRV_FTRCDTTE] DEFAULT ('') NULL,
    [FTRCDTTT] VARCHAR (20)   CONSTRAINT [DF_METLFTRV_FTRCDTTT] DEFAULT ('') NULL,
    [FTRCDTTS] VARCHAR (20)   CONSTRAINT [DF_METLFTRV_FTRCDTTS] DEFAULT ('') NULL,
    [FTRCDUTS] SMALLINT       CONSTRAINT [DF_METLFTRV_FTRCDUTS] DEFAULT ((0)) NULL,
    [FTRDSNOC] VARCHAR (100)  NULL,
    [FTRDSVAL] TEXT           NULL,
    [FTRDSRAP] TEXT           NULL,
    [FTRDSRAT] TEXT           NULL,
    [FTRDSRAI] TEXT           NULL,
    [FTRDSDFG] TEXT           NULL,
    [FTRDSDIP] TEXT           NULL,
    [FTRDSDIT] TEXT           NULL,
    [FTRDSDII] TEXT           NULL,
    [FTRWNNSV] INT            CONSTRAINT [DF_METLFTRV_FTRDSNSV] DEFAULT ((0)) NULL,
    [FTRWNNS1] INT            CONSTRAINT [DF_METLFTRV_FTRDSNS1] DEFAULT ((0)) NULL,
    [FTRWNNS2] INT            CONSTRAINT [DF_METLFTRV_FTRDSNS2] DEFAULT ((0)) NULL,
    [FTRWNNS3] INT            CONSTRAINT [DF_METLFTRV_FTRDSNS3] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_METLFTRV] PRIMARY KEY CLUSTERED ([FTRINNOD] ASC, [FTRINIDI] ASC, [FTRINVER] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No visible en seu electrònica però si en la resta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTRV', @level2type = N'COLUMN', @level2name = N'FTRSWVWE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Visualització de nivell de servei', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTRV', @level2type = N'COLUMN', @level2name = N'FTRWNNSV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTRV', @level2type = N'COLUMN', @level2name = N'FTRWNNS1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTRV', @level2type = N'COLUMN', @level2name = N'FTRWNNS2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de servei 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFTRV', @level2type = N'COLUMN', @level2name = N'FTRWNNS3';

