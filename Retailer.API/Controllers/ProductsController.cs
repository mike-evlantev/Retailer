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
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        //[Route("getall")]
        [Authorize(Roles = "Cashier")]
        public async Task<IEnumerable<IProductModel>> GetAllProducts()
        {
            var repo = new ProductRepository();
            return await repo.GetAllProducts();
        }
    }
}
