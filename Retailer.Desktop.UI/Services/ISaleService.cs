using Retailer.Core.Models;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Services
{
    public interface ISaleService
    {
        Task<int> CreateSale(SaleModel sale);
    }
}