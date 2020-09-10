using Retailer.Core.Helpers;
using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Services
{
    public class SaleService : ISaleService
    {
        private IApiHelper _apiHelper;

        public SaleService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<int> CreateSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sales/CreateSale", sale))
            {
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<int>();
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
