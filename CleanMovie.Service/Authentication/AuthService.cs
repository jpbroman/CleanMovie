using BCrypt.Net;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Interfaces;

namespace CleanMovie.Service.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        await _userRepository.AddUserAsync(user);
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
            return null;

        var validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!validPassword)
            return null;

        return _jwtTokenService.GenerateToken(user);
    }
}
