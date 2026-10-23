using CleanMovie.Core.Entities;
using CleanMovie.Core.Interfaces;

namespace CleanMovie.Service.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
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

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
            return null;

        var validPassword = _passwordHasher.Verify(password, user.PasswordHash);

        if (!validPassword)
            return null;

        return _jwtTokenService.GenerateToken(user);
    }
}
