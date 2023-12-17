using Dapper.Contrib.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
    public interface IDataAccess
    {
        Task<int> ExecuteAsync(string sql, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);

    }

    public class DataAccess(ISqlDbConnFactory sqlDbConnectionFactory) : IDataAccess
    {
        private readonly ISqlDbConnFactory _sqlDbConnectionFactory = sqlDbConnectionFactory;

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using var connection = await _sqlDbConnectionFactory.Connect();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using var connection = await _sqlDbConnectionFactory.Connect();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using var connection = await _sqlDbConnectionFactory.Connect();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
