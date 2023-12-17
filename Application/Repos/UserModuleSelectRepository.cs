using Core.Models;
using Infrastructure.DAL;
using Infrastructure.Loggers;

namespace Application.Repos 
{
    public interface IUserModuleSelectRepository
    {
        Task<UserModuleSelect> GetUserSelectedModulesAsync(int userId);
    }

    public class UserModuleSelectRepository(IDataAccess dataAccess, IAppLogger _logger) : BaseRepository<UserModuleSelect>(dataAccess, _logger), IUserModuleSelectRepository
    {
        private readonly IDataAccess _dataAccess = dataAccess;

        // Additional methods for UserModuleSelect-specific operations can be added here.
        // For example, methods to retrieve the selected modules for a specific user.

        public async Task<UserModuleSelect> GetUserSelectedModulesAsync(int userId)
        {
            string sqlUserModuleSelect = "SELECT * FROM UserModuleSelects WHERE UserID = @UserId";
            string sqlModules = "SELECT * FROM Modules WHERE ModuleID IN (SELECT ModuleID FROM UserModuleSelects WHERE UserID = @UserId)";

            var userModuleSelect = await _dataAccess.QueryFirstOrDefaultAsync<UserModuleSelect>(sqlUserModuleSelect, new { UserId = userId });

            if (userModuleSelect != null)
            {
                userModuleSelect.SelectedModules = (await _dataAccess.QueryAsync<AppModule>(sqlModules, new { UserId = userId })).ToList();
            }

            return userModuleSelect;
        }
    }
}
