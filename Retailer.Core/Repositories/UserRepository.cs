using Retailer.Core.DataAccess;
using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Core.Repositories
{
    public class UserRepository
    {
        SqlDataAccess _sql = new SqlDataAccess();
        private readonly string _connectionStringName = "RetailerData";
        public async Task<UserModel> GetUserByIdAsync(string userId)
        {
            return (await _sql.LoadDataAsync<UserModel, object>("dbo.spUserLookup", new { Id = userId }, _connectionStringName)).SingleOrDefault();

        }
    }
}
