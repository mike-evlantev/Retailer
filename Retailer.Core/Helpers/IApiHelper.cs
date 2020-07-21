using Retailer.Core.Models;
using System.Threading.Tasks;

namespace Retailer.Core.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}