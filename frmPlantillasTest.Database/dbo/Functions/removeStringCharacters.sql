CREATE FUNCTION dbo.removeStringCharacters ( @String1 varchar(8000) )
RETURNS varchar(8000)
AS
BEGIN

    SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#193;', 'Á')--char --> 193
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#201;', 'É')--char --> 201
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#205;', 'Í')--char --> 205
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#211;', 'Ó')--char --> 211
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#218;', 'Ú')--char --> 218
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#224;', 'Ó')
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#225;', 'á')--char --> 225
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#170;', 'ª')
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#233;', 'é')--char --> 233
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#237;', 'í')--char --> 237
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#243;', 'ó')--char --> 243
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#250;', 'ú')--char --> 250	

SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, '&#241;', 'ñ')--char --> 250	
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, char(209), '#')--char --> 209
SELECT @String1=replace(@String1 COLLATE Latin1_General_BIN, char(241), '#')--char --> 241
RETURN @String1
END