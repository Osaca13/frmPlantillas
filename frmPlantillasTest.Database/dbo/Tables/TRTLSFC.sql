CREATE TABLE [dbo].[TRTLSFC] (
    [SFCINLTR] INT          NOT NULL,
    [SFCCDIDI] SMALLINT     CONSTRAINT [DF_TRTLSFC_SFCCDIDI] DEFAULT (1) NOT NULL,
    [SFCDSHOR] VARCHAR (20) NOT NULL,
    [SFCDSTOR] VARCHAR (20) NOT NULL,
    [SFCDSHOC] VARCHAR (50) NOT NULL,
    [SFCDSNCA] VARCHAR (3)  NOT NULL,
    [SFCDSNCU] VARCHAR (3)  NOT NULL,
    [SFCDSPER] CHAR (1)     NOT NULL,
    CONSTRAINT [PK_TRTLSFC] PRIMARY KEY CLUSTERED ([SFCINLTR] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TRTLSFC_METLLTR] FOREIGN KEY ([SFCINLTR]) REFERENCES [dbo].[METLLTR] ([LTRINCOD]) ON DELETE CASCADE
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codi de METLLTR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCINLTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Idioma desitjat de resposta. Per defecte 1 (català)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCCDIDI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipus d’horari del treballador. 
Valors possibles:
31h50’ // 34h55’ // 37h25’
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSHOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Torn de treball
Valors possibles:
Matí / Matí i tarda / Tarda / Nit / Obert
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSTOR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Horari del curs sol•licitat:
De 8:30h a 10:30h, dimarts i dijous
De 13h a 15h, dilluns i dimecres
De 15:30h a 17:30h, dilluns i dimecres
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSHOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de català actual:
B1 / B2 / B3 / E1 / E2 / E3 / I1 / I2 / I3 / S1 / S2 / S3
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSNCA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nivell de curs sol•licitat
B1 / B2 / B3 / E1 / E2 / E3 / I1 / I2 / I3 / S1 / S2 / S3
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSNCU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Perfil lingüístic del lloc de treball
A / B / C
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TRTLSFC', @level2type = N'COLUMN', @level2name = N'SFCDSPER';

