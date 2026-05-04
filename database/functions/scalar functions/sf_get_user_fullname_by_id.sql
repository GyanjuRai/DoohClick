
CREATE OR ALTER FUNCTION [identity].sf_get_user_fullname_by_id
(
	@Id INT
)
RETURNS NVARCHAR(100)
AS
BEGIN
	
	DECLARE @FullName NVARCHAR(100) = N'';
	
	SELECT @FullName = CONCAT(u.[name], ' ', ISNULL(u.sur_name, ''))
	FROM [identity].[user] AS u
	WHERE u.id = @Id;

	RETURN @FullName;
END;
