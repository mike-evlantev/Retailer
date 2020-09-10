CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@Id INT,
	@SaleId INT,
	@ProductId INT,
	@Quantity INT,
	@PurchasePrice MONEY,
	@Tax MONEY
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [dbo].[SaleDetail](SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES(@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax)
END
