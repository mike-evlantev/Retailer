using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Helpers
{
    // To read from App.config
    public class ConfigHelper : IConfigHelper
    {
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
