using System;
using System.Configuration;

namespace Retailer.Core.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO: Move to API or DB?
        public decimal GetTaxRate()
        {
            var isValidTaxRate = Decimal.TryParse(
                                            ConfigurationManager.AppSettings["taxRate"],
                                            out decimal taxRate);
            if (!isValidTaxRate)
                throw new ConfigurationErrorsException("Failed to get valid tax rate");

            return taxRate;
        }
    }
}
