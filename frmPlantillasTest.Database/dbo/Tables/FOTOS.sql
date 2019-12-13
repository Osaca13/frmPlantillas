CREATE TABLE [dbo].[FOTOS] (
    [Numero]     NUMERIC (18)   NOT NULL,
    [Data]       CHAR (10)      NULL,
    [Format]     NVARCHAR (4)   NULL,
    [Imatge]     NVARCHAR (100) NULL,
    [NomAutor]   NVARCHAR (38)  NULL,
    [Us]         NVARCHAR (40)  NULL,
    [Panoramica] NVARCHAR (2)   NULL,
    [Qualitat]   NVARCHAR (1)   NULL,
    [Barri]      SMALLINT       NULL,
    [Quantitat]  FLOAT (53)     NULL,
    [Temes]      TEXT           NULL,
    CONSTRAINT [PK_FOTOS] PRIMARY KEY CLUSTERED ([Numero] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FOTOS_BARRIS] FOREIGN KEY ([Barri]) REFERENCES [dbo].[BARRIS] ([BARINBAR])
);

