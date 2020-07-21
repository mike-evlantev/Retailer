CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
BEGIN
	SELECT Id, FirstName, LastName, Email, CreatedDate
	FROM [dbo].[User]
	WHERE Id = @Id
END