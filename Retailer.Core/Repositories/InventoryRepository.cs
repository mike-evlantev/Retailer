using Retailer.Core.DataAccess;
using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Core.Repositories
{
    public class InventoryRepository
    {
        SqlDataAccess _sql = new SqlDataAccess();
        private readonly string _connectionStringName = "RetailerData";

        public async Task<IEnumerable<InventoryItemModel>> GetInventory() =>
            await _sql.QueryAsync<InventoryItemModel, object>(
                $@"
                	SELECT I.*, P.[Name] AS [ProductName]
                    FROM [dbo].[Inventory] I
                    INNER JOIN [dbo].[Product] P
                        ON I.ProductId = P.Id 
                ",
                null,
                _connectionStringName);

        public async Task<int> AddInventoryItem(InventoryItemModel item) =>
            await _sql.ExecuteWithOutputAsync(
                $@"
                	INSERT INTO [dbo].[Inventory](ProductId, Quantity, PurchasePrice, PurchaseDate)
                    OUTPUT INSERTED.[Id]
                    VALUES(
                        @{nameof(item.ProductId)}, 
                        @{nameof(item.Quantity)}, 
                        @{nameof(item.PurchasePrice)}, 
                        @{nameof(item.PurchaseDate)})
                ",
                new 
                {
                    item.ProductId, 
                    item.Quantity, 
                    item.PurchasePrice,
                    item.PurchaseDate
                },
                _connectionStringName);
    }
}
