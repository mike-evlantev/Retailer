using Retailer.Core.Models;
using Retailer.Desktop.UI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
    }
}