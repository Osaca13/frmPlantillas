﻿CREATE TABLE [dbo].[CCITLHOC] (
    [HOCINEXP] INT       NOT NULL,
    [HOCSTCPA] CHAR (10) NOT NULL,
    CONSTRAINT [FK_CCITLHOC_METLLTR] FOREIGN KEY ([HOCINEXP]) REFERENCES [dbo].[METLLTR] ([LTRINCOD])
);

