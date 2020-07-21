using Newtonsoft.Json;

namespace Retailer.Core.Models
{
    public class AuthenticatedUser
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
