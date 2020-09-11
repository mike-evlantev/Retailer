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
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiController
    {
        private readonly InventoryRepository _inventoryRepository = new InventoryRepository();

        [HttpPost]
        [Route("addItem")]
        public async Task<int> AddInventoryItem(InventoryItemModel item)
        {
            return await _inventoryRepository.AddInventoryItem(item);
        }

        [HttpGet]
        public async Task<IEnumerable<InventoryItemModel>> GetInventory()
        {
            return await _inventoryRepository.GetInventory();
        }
    }
}
