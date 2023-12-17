using Core.Models;
using Infrastructure.DAL;
using Infrastructure.Loggers;

namespace Application.Repos 
{
    public interface IAppModuleRepository
    {
        Task<AppModule> GetByModuleId(int moduleId);
        Task<IEnumerable<AppModule>> GetByUserId(int userId);
        Task<IEnumerable<AppModule>> GetModulesByUserId(int userId);
        Task<bool> UpdateUserModules(int userId, List<int> selectedModuleIds); 
    }

    public class AppModuleRepository : BaseRepository<AppModule>, IAppModuleRepository
    {
        private readonly IDataAccess _dataAccess;

        public AppModuleRepository(IDataAccess dataAccess, IAppLogger logger)
            : base(dataAccess, logger)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<AppModule>> GetModulesByUserId(int userId)
        {
            string sqlModules = @"SELECT m.*, 
                                       CASE WHEN um.ModuleID IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsChosen
                                FROM Modules m
                                LEFT JOIN UserModules um ON m.ID = um.ModuleID AND um.UserID = @UserID";

            string sqlComponents = @"SELECT c.* FROM Components c
                                INNER JOIN ModuleComponents mc ON c.ID = mc.ComponentID
                                WHERE mc.ModuleID = @ModuleID";

            var modules = await _dataAccess.QueryAsync<AppModule>(sqlModules, new { UserID = userId });

            foreach (var module in modules)
            {
                var components = await _dataAccess.QueryAsync<AppComponent>(sqlComponents, new { ModuleID = module.ID });
                module.Components = components.ToList();
            }

            return modules;
        }

        public async Task<bool> UpdateUserModules(int userId, List<int> selectedModuleIds) 
        {
            // Step 1: Remove existing selections for the user
            string sqlDeleteExisting = "DELETE FROM UserModules WHERE UserID = @UserID";
            await _dataAccess.ExecuteAsync(sqlDeleteExisting, new { UserID = userId });

            // Step 2: Insert new selections for the user
            foreach (var moduleId in selectedModuleIds)
            {
                string sqlInsert = "INSERT INTO UserModules (UserID, ModuleID) VALUES (@UserID, @ModuleID)";
                await _dataAccess.ExecuteAsync(sqlInsert, new { UserID = userId, ModuleID = moduleId });
            }
            return true;
        }


        public async Task<IEnumerable<AppModule>> GetByUserId(int userId)
        {
            string sql = @"
            SELECT m.* FROM Modules m
            JOIN UserModules um ON m.ID = um.ModuleID
            WHERE um.UserID = @UserID";

            return await _dataAccess.QueryAsync<AppModule>(sql, new { UserID = userId });
        }

        public async Task<AppModule> GetByModuleId(int moduleId)
        {
            string sql = "SELECT * FROM Modules WHERE ID = @ModuleID";
            return await _dataAccess.QueryFirstOrDefaultAsync<AppModule>(sql, new { ModuleID = moduleId });
        }
    }

}
