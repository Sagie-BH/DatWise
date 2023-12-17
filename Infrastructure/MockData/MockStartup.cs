using Core.Models;
using Dapper;
using Infrastructure.DAL;
using System.Data;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.MockData
{
    public interface IMockStartup
    {
        ValueTask<bool> CreateTables();
        ValueTask<bool> InsertMockData();
    }

    public class MockStartup : IMockStartup
    {
        private readonly IDataAccess _dataAccess;

        public MockStartup(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async ValueTask<bool> CreateTables()
        {
            // Users table creation
            if (!await DoesTableExist("Users"))
            {
                var createUsersTable = @"
                    CREATE TABLE Users (
                        ID INT PRIMARY KEY, 
                        Name NVARCHAR(100) NOT NULL,
                        Company NVARCHAR(100) NOT NULL, 
                        Email NVARCHAR(100) NOT NULL, 
                        Password NVARCHAR(100) NOT NULL, 
                        IsActive BIT
                    )";
                await _dataAccess.ExecuteAsync(createUsersTable);
            }

            // Modules table creation
            if (!await DoesTableExist("Modules"))
            {
                var createModulesTable = @"
                    CREATE TABLE Modules (
                        ID INT PRIMARY KEY, 
                        Title NVARCHAR(100) NOT NULL,
                        Image NVARCHAR(MAX),
                        Icon NVARCHAR(MAX),
                        Description NVARCHAR(MAX)
                    )";
                await _dataAccess.ExecuteAsync(createModulesTable);
            }

            // Components table creation
            if (!await DoesTableExist("Components"))
            {
                var createComponentsTable = @"
                    CREATE TABLE Components (
                        ID INT PRIMARY KEY, 
                        Title NVARCHAR(100), 
                        Description NVARCHAR(MAX)
                    )";
                await _dataAccess.ExecuteAsync(createComponentsTable);
            }

            // Junction table for Modules and Components
            if (!await DoesTableExist("ModuleComponents"))
            {
                var createModuleComponentsTable = @"
                    CREATE TABLE ModuleComponents (
                        ModuleID INT,
                        ComponentID INT,
                        PRIMARY KEY (ModuleID, ComponentID),
                        FOREIGN KEY (ModuleID) REFERENCES Modules(ID),
                        FOREIGN KEY (ComponentID) REFERENCES Components(ID)
                    )";
                await _dataAccess.ExecuteAsync(createModuleComponentsTable);
            }

            // Junction table for UserModuleSelect
            if (!await DoesTableExist("UserModules"))
            {
                var createUserModulesTable = @"
                    CREATE TABLE UserModules (
                        UserID INT,
                        ModuleID INT,
                        PRIMARY KEY (UserID, ModuleID),
                        FOREIGN KEY (UserID) REFERENCES Users(ID),
                        FOREIGN KEY (ModuleID) REFERENCES Modules(ID)
                        )";
                await _dataAccess.ExecuteAsync(createUserModulesTable);
            }

            return true;
        }



        private async ValueTask<bool> DoesTableExist(string tableName)
        {
            var sql = @"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @TableName";
            var parameters = new DynamicParameters();
            parameters.Add("TableName", tableName, DbType.String);

            var result = await _dataAccess.QueryFirstOrDefaultAsync<int?>(sql, parameters);
            return result.HasValue;
        }

        public async ValueTask<bool> InsertMockData()
        {
            // Deserialize JSON files and insert data into the database
            var components = await ReadJsonFile<AppComponent>("MockLists/components_data.json");
            var modules = await ReadJsonFile<AppModule>("MockLists/modules_components.json");
            var users = await ReadJsonFile<AppUser>("MockLists/users_data.json");
            var userModuleSelects = await ReadJsonFile<UserModuleSelect>("MockLists/user_module_select_data.json");

            await InsertComponents(components);
            await InsertModules(modules);
            await InsertUsers(users);
            await InsertUserModuleSelects(userModuleSelects);

            return true;
        }

        private async ValueTask<List<T>> ReadJsonFile<T>(string filePath)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(basePath, filePath);

            var options = new JsonSerializerOptions
            {
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            using var stream = File.OpenRead(fullPath);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream, options) ?? new List<T>();
        }


        private async Task InsertComponents(List<AppComponent> components)
        {
            var sql = @"INSERT INTO Components (ID, Title, Description) VALUES (@ID, @Title, @Description)";
            foreach (var component in components)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", component.ID);
                parameters.Add("@Title", component.Title);
                parameters.Add("@Description", component.Description);

                await _dataAccess.ExecuteAsync(sql, parameters);
            }
        }


        private async Task InsertModules(List<AppModule> modules)
        {
            var moduleInsertSql = @"INSERT INTO Modules (ID, Title, Image, Icon, Description) VALUES (@ID, @Title, @Image, @Icon, @Description)";
            var moduleComponentInsertSql = @"INSERT INTO ModuleComponents (ModuleID, ComponentID) VALUES (@ModuleID, @ComponentID)";

            foreach (var module in modules)
            {
                var moduleParams = new DynamicParameters();
                moduleParams.Add("@ID", module.ID);
                moduleParams.Add("@Title", module.Title);
                moduleParams.Add("@Image", module.Image);
                moduleParams.Add("@Icon", module.Icon);
                moduleParams.Add("@Description", module.Description);

                await _dataAccess.ExecuteAsync(moduleInsertSql, moduleParams);

                // Inserting records into ModuleComponents junction table
                foreach (var component in module.Components)
                {
                    var moduleComponentParams = new DynamicParameters();
                    moduleComponentParams.Add("@ModuleID", module.ID);
                    moduleComponentParams.Add("@ComponentID", component.ID);

                    await _dataAccess.ExecuteAsync(moduleComponentInsertSql, moduleComponentParams);
                }
            }
        }



        private async Task InsertUsers(List<AppUser> users)
        {
            var sql = @"INSERT INTO Users (ID, Name, Company, Password, Email, IsActive) VALUES (@ID, @Name, @Company, @Password, @Email, @IsActive)";
            foreach (var user in users)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", user.ID);
                parameters.Add("@Name", user.Name);
                parameters.Add("@Company", user.Company);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Password", user.Password);
                parameters.Add("@IsActive", user.IsActive);

                await _dataAccess.ExecuteAsync(sql, parameters);
            }
        }

        private async Task InsertUserModuleSelects(List<UserModuleSelect> userModuleSelects)
        {
            var userModulesInsertSql = @"INSERT INTO UserModules (UserID, ModuleID) VALUES (@UserID, @ModuleID)";

            foreach (var userModuleSelect in userModuleSelects)
            {
                foreach (var module in userModuleSelect.SelectedModules)
                {
                    if (!await UserModuleExists(userModuleSelect.UserID, module.ID)) // Assuming module.ID is the correct property
                    {
                        var userModulesParams = new DynamicParameters();
                        userModulesParams.Add("@UserID", userModuleSelect.UserID);
                        userModulesParams.Add("@ModuleID", module.ID); // Assuming module.ID is the correct property

                        await _dataAccess.ExecuteAsync(userModulesInsertSql, userModulesParams);
                    }
                }
            }
        }


        private async Task<bool> UserModuleExists(int userId, int moduleId)
        {
            var sql = @"SELECT COUNT(1) FROM UserModules WHERE UserID = @UserID AND ModuleID = @ModuleID";
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            parameters.Add("@ModuleID", moduleId);

            int count = await _dataAccess.QueryFirstOrDefaultAsync<int>(sql, parameters);
            return count > 0;
        }


    }

}
