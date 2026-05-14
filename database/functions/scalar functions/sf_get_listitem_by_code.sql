
CREATE OR ALTER FUNCTION shared.sf_get_listitem_by_code
(
	@Code NVARCHAR(50)
)
RETURNS NVARCHAR(50)
AS
BEGIN
	
	DECLARE @Item NVARCHAR(50);

	SELECT @Item = ISNULL(LOWER(item), '')
	FROM shared.listitem 
	WHERE code = @Code;
	
	RETURN @Item;
END