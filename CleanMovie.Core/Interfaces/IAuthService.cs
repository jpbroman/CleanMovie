using CleanMovie.Core.Entities;

namespace CleanMovie.Core.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string username, string password, string role = "User");

    Task<string?> LoginAsync(string username, string password);
}
