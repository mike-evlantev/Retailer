CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id INT OUTPUT,
	@UserId NVARCHAR(128),
	@CreatedDate DATETIME2,
	@Subtotal MONEY,
	@Tax MONEY,
	@Total MONEY
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [dbo].[Sale](UserId, CreatedDate, Subtotal, Tax, Total)
	VALUES(@UserId, @CreatedDate, @Subtotal, @Tax, @Total)

	SELECT @Id = @@IDENTITY
END
