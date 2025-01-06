using UserAPI.DTO;
using UserAPI.Models;

namespace UserAPI.Interfaces
{
    public interface IUserService
    {
        public Task<User?> CreateUserAsync(User newUser);

        public Task<User?> GetUserAsyncByUsername(string username);

        public Task<User?> GetUserAsync(string id);

        public Task<User> UpdateUserAsync(string id, UpdateUser updatedUser);

        public Task<bool> DeleteUserAsync(string id);

        public Task<User> LoginUser(string username, string password);

        public Task<User> LoginUserWithGoogle(string username);
    }
}
