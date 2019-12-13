CREATE TABLE [dbo].[INTLEST] (
    [ESTCDEST] CHAR (4)  NOT NULL,
    [ESTDSDES] CHAR (50) NOT NULL,
    [ESTDSGRP] CHAR (2)  NOT NULL,
    [ESTCDORD] INT       NULL,
    [ESTDSDGR] CHAR (20) NULL,
    CONSTRAINT [PK_INTLEST] PRIMARY KEY CLUSTERED ([ESTCDEST] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de estado de la reserva', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLEST', @level2type = N'COLUMN', @level2name = N'ESTCDEST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del estado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'INTLEST', @level2type = N'COLUMN', @level2name = N'ESTDSDES';

