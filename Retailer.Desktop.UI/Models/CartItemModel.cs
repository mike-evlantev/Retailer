using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Models
{
    public class CartItemModel
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
