using Infrastructure;

namespace MockStartup
{
    public interface IMockStartup
    {
        ValueTask<bool> CreateTables();
    }

    public class MockStartup : IMockStartup
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MockStartup(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async ValueTask<bool> CreateTables()
        {
            // Create Users table
            var createUsersTable = @"CREATE TABLE Users (UserID INT PRIMARY KEY, Username NVARCHAR(100), Password NVARCHAR(100), IsActive BIT)";
            await _sqlDataAccess.ExecuteSqlText(createUsersTable);

            // Create Modules table
            var createModulesTable = @"CREATE TABLE Modules (ModuleID INT PRIMARY KEY, ModuleName NVARCHAR(100), Header NVARCHAR(100), Description NVARCHAR(MAX))";
            await _sqlDataAccess.ExecuteSqlText(createModulesTable);

            // Create Components table
            var createComponentsTable = @"CREATE TABLE Components (ComponentID INT PRIMARY KEY, ComponentName NVARCHAR(100), Description NVARCHAR(MAX))";
            await _sqlDataAccess.ExecuteSqlText(createComponentsTable);

            // Create UserModuleSelect table
            var createUserModuleSelectTable = @"CREATE TABLE UserModuleSelect (UserID INT, ModuleID INT, FOREIGN KEY (UserID) REFERENCES Users(UserID), FOREIGN KEY (ModuleID) REFERENCES Modules(ModuleID))";
            await _sqlDataAccess.ExecuteSqlText(createUserModuleSelectTable);

            return true;
        }
    }
}
