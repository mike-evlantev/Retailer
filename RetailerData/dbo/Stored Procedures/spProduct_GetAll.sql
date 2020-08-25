CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
BEGIN
	SELECT * 
	FROM [dbo].[Product]
	ORDER BY [Name]
END
