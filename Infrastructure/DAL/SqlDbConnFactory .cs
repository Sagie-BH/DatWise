using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DAL
{
    public interface ISqlDbConnFactory
    {
        ValueTask<IDbConnection?> Connect();
        ValueTask<bool> CheckConnectionAsync();
    }

    public class SqlDbConnFactory(IConfiguration config) : ISqlDbConnFactory
    {
        private readonly IConfiguration _config = config;

        public async ValueTask<IDbConnection?> Connect()
        {
            var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return conn;
        }
        public async ValueTask<bool> CheckConnectionAsync()
        {
            try
            {
                using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                await conn.OpenAsync();
                return conn.State == ConnectionState.Open;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
