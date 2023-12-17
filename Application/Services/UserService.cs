using Application.Repos;
using Core.Models;

namespace Application.Services
{
    public interface IUserService
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> ValidateUserCredentialsAsync(string username, string email);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<AppUser> ValidateUserCredentialsAsync(string email, string password)
        {
            return await _userRepository.ValidateUserCredentialsAsync(email, password);
        }
    }
}
