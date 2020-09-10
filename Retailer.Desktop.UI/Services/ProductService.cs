using Retailer.Core.Helpers;
using Retailer.Core.Models;
using Retailer.Desktop.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Services
{
    public class ProductService : IProductService
    {
        private IApiHelper _apiHelper;

        public ProductService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Products"))
            {
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<IEnumerable<ProductModel>>();
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
