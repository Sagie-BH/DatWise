using Core.Models;
using Infrastructure.DAL;
using Infrastructure.Loggers;

namespace Application.Repos
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> ValidateUserCredentialsAsync(string email, string password);
    }

    public class UserRepository(IDataAccess dataAccess, IAppLogger _logger) : BaseRepository<AppUser>(dataAccess, _logger), IUserRepository
    {
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username";
            return await GetByIdAsync(sql, new { Username = username });
        }

        public async Task<AppUser> ValidateUserCredentialsAsync(string email, string password)
        {
            string sql = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
            var user = await GetByIdAsync(sql, new { Email = email, Password = password });
            return user;
        }

    }

}
