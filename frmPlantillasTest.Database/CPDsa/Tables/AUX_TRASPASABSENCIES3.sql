﻿CREATE TABLE [CPDsa].[AUX_TRASPASABSENCIES3] (
    [ID_EMPRESA]     NCHAR (5)   NOT NULL,
    [NIF]            NCHAR (14)  NOT NULL,
    [F_INI_APUNTE]   NCHAR (16)  NOT NULL,
    [HORAS_INI]      NCHAR (5)   NULL,
    [F_FIN_APUNTE]   NCHAR (16)  NOT NULL,
    [HORAS_FIN]      NCHAR (5)   NULL,
    [ID_TIP_APUNTE]  NCHAR (5)   NOT NULL,
    [ID_TIP_GESTION] NCHAR (3)   NOT NULL,
    [ID_PROCESO_IT]  NCHAR (3)   NULL,
    [FECHAACC]       NCHAR (10)  NULL,
    [N_PARTE]        NCHAR (5)   NULL,
    [ID_DIAGTRA]     NCHAR (10)  NULL,
    [ID_MEDICO]      NCHAR (14)  NULL,
    [N_COLEGIADO]    NCHAR (10)  NULL,
    [ID_MUTUA]       NCHAR (3)   NULL,
    [A_SANITARIA]    NCHAR (11)  NULL,
    [F_CAMBIOTEP]    NCHAR (16)  NULL,
    [D_SUCESO_ACC]   NCHAR (60)  NULL,
    [NOTIF_BUZON]    CHAR (1)    NULL,
    [OBSERVACIONES]  NCHAR (500) NULL
);
