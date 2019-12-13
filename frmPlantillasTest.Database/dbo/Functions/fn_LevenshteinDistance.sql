CREATE FUNCTION [dbo].[fn_LevenshteinDistance]
(@firstString NVARCHAR (4000), @secondString NVARCHAR (4000), @ingoreCase BIT=1)
RETURNS INT
AS
 EXTERNAL NAME [fuzzyStrings].[FuzzyStrings].[LevenshteinDistance]

