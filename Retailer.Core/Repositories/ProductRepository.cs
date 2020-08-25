using Retailer.Core.DataAccess;
using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Core.Repositories
{
    public class ProductRepository
    {
        SqlDataAccess _sql = new SqlDataAccess();
        private readonly string _connectionStringName = "RetailerData";

        public async Task<IEnumerable<IProductModel>> GetAllProducts() => 
            await _sql.LoadDataAsync<ProductModel, object>(
                "dbo.spProduct_GetAll",
                null,
                _connectionStringName);
    }
}
