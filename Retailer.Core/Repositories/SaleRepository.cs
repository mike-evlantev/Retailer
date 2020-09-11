using Retailer.Core.DataAccess;
using Retailer.Core.DataAccess.Models;
using Retailer.Core.Helpers;
using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Core.Repositories
{
    public class SaleRepository
    {
        ConfigHelper _config = new ConfigHelper();
        SqlDataAccess _sql;
        ProductRepository _productRepository = new ProductRepository();
        private readonly string _connectionStringName = "RetailerData";

        public async Task<int> CreateSaleAsync(string userId, SaleModel saleInfo)
        {
            decimal taxRate = 0.0875M;//new ConfigHelper().GetTaxRate();
            var saleDetails = new List<SaleDetailDbModel>();
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (productInfo == null)
                    throw new Exception($"The product Id {item.ProductId} could not be found");

                detail.PurchasePrice = (decimal)(productInfo.RetailPrice * item.Quantity);

                if (productInfo.IsTaxable)
                    detail.Tax = (detail.PurchasePrice * taxRate);

                saleDetails.Add(detail);
            }

            var subtotal = saleDetails.Sum(d => d.PurchasePrice);
            var tax = saleDetails.Sum(d => d.Tax);
            var total = subtotal + tax;

            var saleModel = new SaleDbModel()
            {
                Subtotal = subtotal,
                Tax = tax,
                Total = total,
                UserId = userId
            };

            var saleId = -1;
            using (_sql = new SqlDataAccess())
            {
                try
                {
                    _sql.StartTransaction(_connectionStringName);
                    saleId = await _sql.ExecuteWithOutputInTransactionAsync(
                    $@"
                	    INSERT INTO [dbo].[Sale](UserId, Subtotal, Tax, Total)
                        OUTPUT INSERTED.[Id]
                        VALUES(
                            @{nameof(saleModel.UserId)}, 
                            @{nameof(saleModel.Subtotal)}, 
                            @{nameof(saleModel.Tax)}, 
                            @{nameof(saleModel.Total)})
                    ",
                    new
                    {
                        saleModel.UserId,
                        saleModel.Subtotal,
                        saleModel.Tax,
                        saleModel.Total
                    });

                    foreach (var detail in saleDetails)
                    {
                        detail.SaleId = saleId;
                        // TODO: Single Call? TVP
                        await _sql.ExecuteInTransactionAsync(
                        $@"
                            INSERT INTO [dbo].[SaleDetail](SaleId, ProductId, Quantity, PurchasePrice, Tax)
                            VALUES(
                                @{nameof(detail.SaleId)}, 
                                @{nameof(detail.ProductId)}, 
                                @{nameof(detail.Quantity)}, 
                                @{nameof(detail.PurchasePrice)}, 
                                @{nameof(detail.Tax)})
                        ",
                            new
                            {
                                detail.SaleId,
                                detail.ProductId,
                                detail.Quantity,
                                detail.PurchasePrice,
                                detail.Tax
                            });
                    }

                    _sql.CommitTransaction();
                }
                catch
                {
                    _sql.RollbackTransaction();
                    throw;
                }
            }

            return saleId;
        }
    }
}
