﻿CREATE TABLE [dbo].[METLPLT] (
    [PLTINNOD] INT            NOT NULL,
    [PLTDSTIT] TEXT           NULL,
    [PLTDTANY] DATETIME       NULL,
    [PLTDSUSR] CHAR (10)      NULL,
    [PLTDSHOR] TEXT           NULL,
    [PLTDSVER] TEXT           NULL,
    [PLTDSCMP] TEXT           NULL,
    [PLTDSEST] TEXT           NULL,
    [PLTDSATR] TEXT           NULL,
    [PLTDSCSS] VARCHAR (8000) NULL,
    [PLTDSLNK] TEXT           NULL,
    [PLTDSIMG] TEXT           NULL,
    [PLTDSTCO] TEXT           NULL,
    [PLTDSLCW] TEXT           NOT NULL,
    [PLTDSLC2] TEXT           NOT NULL,
    [PLTDSALK] TEXT           NULL,
    [PLTCDPAL] TEXT           NULL,
    [PLTDSAAL] TEXT           NULL,
    [PLTDSALT] TEXT           CONSTRAINT [DF_METLPLTNOU_PLTDSALT] DEFAULT ('') NULL,
    [PLTDSPLT] TEXT           NULL,
    [PLTDSFLW] TEXT           CONSTRAINT [DF_METLPLTNOU_PLTDSFLW] DEFAULT ('9') NOT NULL,
    [PLTDSOBS] VARCHAR (250)  NULL,
    [PLTSWALT] SMALLINT       CONSTRAINT [DF_METLPLTNOU_PLTSWALT] DEFAULT (1) NOT NULL,
    [PLTDSNUM] TEXT           NULL,
    [PLTDSALF] TEXT           NULL,
    [PLTDSNIV] TEXT           NULL,
    [PLTSWVIS] SMALLINT       NULL,
    CONSTRAINT [PK_METLPLTNOU] PRIMARY KEY CLUSTERED ([PLTINNOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inclore dins de plantilles alternatives?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLPLT', @level2type = N'COLUMN', @level2name = N'PLTSWALT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre d''elements a representar', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLPLT', @level2type = N'COLUMN', @level2name = N'PLTDSNUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llista de targets destí d''un contingut de tipus auto-link (0:self, 1:blank)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLPLT', @level2type = N'COLUMN', @level2name = N'PLTDSALF';

