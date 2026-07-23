using CleanMovie.Core.Entities;
using CleanMovie.Core.Interfaces;

namespace CleanMovie.Service.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterAsync(
        string username,
        string password,
        string role = "User")
    {
        if (await _userRepository.UserExistsAsync(username))
            throw new InvalidOperationException("Username already exists.");

        var user = new User
        {
            Username = username,
            PasswordHash = _passwordHasher.Hash(password),
            Role = role
        };

        await _userRepository.AddUserAsync(user);
    }

    public async Task<User?> AuthenticateAsync(
        string username,
        string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
            return null;

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}
