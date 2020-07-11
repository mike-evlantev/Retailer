using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Models
{
    public class AuthenticatedUser
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
