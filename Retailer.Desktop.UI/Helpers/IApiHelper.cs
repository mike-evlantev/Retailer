using Retailer.Desktop.UI.Models;
using System.Threading.Tasks;

namespace Retailer.Desktop.UI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
    }
}