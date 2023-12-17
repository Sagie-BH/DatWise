using Infrastructure.Loggers;

namespace Infrastructure.DAL
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> DeleteAsync(string sql, object parameters);
        Task<IEnumerable<T>> GetAllAsync(string sql);
        Task<T> GetByIdAsync(string sql, object parameters);
        Task<int> InsertAsync(string sql, T entity);
        Task<int> UpdateAsync(string sql, T entity);
    }

    public class BaseRepository<T>(IDataAccess dataAccess, IAppLogger logger) : IBaseRepository<T> where T : class
    {
        private readonly IDataAccess _dataAccess = dataAccess;
        protected readonly IAppLogger _logger = logger;

        public async Task<IEnumerable<T>> GetAllAsync(string sql)
        {
            return await _dataAccess.QueryAsync<T>(sql);
        }

        public async Task<T> GetByIdAsync(string sql, object parameters)
        {
            return await _dataAccess.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> InsertAsync(string sql, T entity)
        {
            return await _dataAccess.ExecuteAsync(sql, entity);
        }

        public async Task<int> UpdateAsync(string sql, T entity)
        {
            return await _dataAccess.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(string sql, object parameters)
        {
            return await _dataAccess.ExecuteAsync(sql, parameters);
        }
    }
}
