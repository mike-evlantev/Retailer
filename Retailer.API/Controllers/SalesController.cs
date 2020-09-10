using Microsoft.AspNet.Identity;
using Retailer.API.Models;
using Retailer.Core.Models;
using Retailer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Retailer.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sales")]
    public class SalesController : ApiController
    {
        [Route("createSale")]
        public async Task<int> CreateSale(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var saleRepository = new SaleRepository();
            return await saleRepository.CreateSaleAsync(userId, sale);
        }
    }
}
