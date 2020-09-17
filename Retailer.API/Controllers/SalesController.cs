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
        private readonly SaleRepository _saleRepository = new SaleRepository();

        [HttpPost]
        [Route("createSale")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<int> CreateSaleAsync(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            return await _saleRepository.CreateSaleAsync(userId, sale);
        }

        [HttpGet]
        [Route("getUsersSales")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IEnumerable<UserSaleModel>> GetUsersSalesAsync()
        {
            return await _saleRepository.GetAllUsersSalesAsync();
        }
    }
}
