using Retailer.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Retailer.Core.Helpers
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
        Task GetLoggedInUserInfo(string token);
        void ClearClient();
    }
}