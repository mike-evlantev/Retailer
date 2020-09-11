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
    internal class SqlDataAccess : IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _isClosed = false;

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

        public async Task<int> SaveDataWithOutputAsync<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows;
            }
        }

        public async Task<IEnumerable<T>> QueryInTransactionAsync<T, U>(string storedProcedure, U parameters)
            => await _connection.QueryAsync<T>(
                storedProcedure, 
                parameters, 
                commandType: CommandType.StoredProcedure, 
                transaction: _transaction);

        public async Task ExecuteAsync<T>(string sql, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<int> ExecuteWithOutputAsync<T>(string sql, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return await connection.QuerySingleAsync<int>(sql, parameters);
            }
        }

        public async Task ExecuteInTransactionAsync<T>(string sql, T parameters) 
            => await _connection.ExecuteAsync(
                sql, 
                parameters, 
                transaction: _transaction);

        public async Task<int> ExecuteWithOutputInTransactionAsync<T>(string sql, T parameters)
            => await _connection.QuerySingleAsync<int>(
                sql, 
                parameters,
                transaction: _transaction);

        // Open connection/start transaction
        public void StartTransaction(string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _isClosed = false;
        }

        // Close connection/stop transaction
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            _isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            _isClosed = true;
        }

        // Dispose
        public void Dispose()
        {
            if (!_isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    throw;
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
