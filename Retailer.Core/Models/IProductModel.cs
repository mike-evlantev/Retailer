using System;

namespace Retailer.Core.Models
{
    public interface IProductModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        double RetailPrice { get; set; }
        int InStock { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime LastModified { get; set; }
        bool IsTaxable { get; set; }
    }
}