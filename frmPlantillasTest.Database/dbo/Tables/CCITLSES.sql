CREATE TABLE [dbo].[CCITLSES] (
    [SESINCOD] INT          IDENTITY (1, 1) NOT NULL,
    [SESDTPRI] DATETIME     NOT NULL,
    [SESDTULT] DATETIME     NOT NULL,
    [SESCDUSU] VARCHAR (21) NOT NULL,
    CONSTRAINT [PK_CCITLSES] PRIMARY KEY CLUSTERED ([SESINCOD] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'codi de sessió', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CCITLSES', @level2type = N'COLUMN', @level2name = N'SESINCOD';

