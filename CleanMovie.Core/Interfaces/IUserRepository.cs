using CleanMovie.Core.Entities;

namespace CleanMovie.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);

    Task<bool> UserExistsAsync(string username);
    
}
