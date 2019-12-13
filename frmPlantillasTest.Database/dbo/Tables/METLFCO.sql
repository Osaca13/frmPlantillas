CREATE TABLE [dbo].[METLFCO] (
    [FCOINNOD] INT      NOT NULL,
    [FCOINIDI] INT      NOT NULL,
    [FCODSTIT] TEXT     NOT NULL,
    [FCODSINT] CHAR (1) CONSTRAINT [DF_METLFCO_FCODSINT] DEFAULT ('S') NOT NULL,
    CONSTRAINT [PK_METLFCO] PRIMARY KEY CLUSTERED ([FCOINNOD] ASC, [FCOINIDI] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFCO', @level2type = N'COLUMN', @level2name = N'FCOINNOD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Iidioma', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFCO', @level2type = N'COLUMN', @level2name = N'FCOINIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text amb el titol de la codificació', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFCO', @level2type = N'COLUMN', @level2name = N'FCODSTIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Us exlusiu a Intranet?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'METLFCO', @level2type = N'COLUMN', @level2name = N'FCODSINT';

