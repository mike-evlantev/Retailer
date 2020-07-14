using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Core.DataAccess
{
    public class SqlDataAccess
    {
        public string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rows;
            }
        }

        public async Task SaveDataAsync<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
