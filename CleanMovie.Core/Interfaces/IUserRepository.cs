using CleanMovie.Core.Entities;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);

    Task<bool> UserExistsAsync(string username);
    
}
