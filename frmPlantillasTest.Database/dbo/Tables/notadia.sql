CREATE TABLE [dbo].[notadia] (
    [nro]      INT          IDENTITY (1, 1) NOT NULL,
    [tipus]    CHAR (1)     NOT NULL,
    [data]     VARCHAR (50) NULL,
    [titol]    TEXT         NULL,
    [resum]    TEXT         NULL,
    [texto]    TEXT         NULL,
    [acte]     TEXT         NULL,
    [dataacte] TEXT         NULL,
    [llocacte] TEXT         NULL,
    [titol2]   TEXT         NULL,
    [nrocom]   INT          NULL,
    [horaacte] TEXT         NULL,
    [idioma]   CHAR (1)     NULL
);

