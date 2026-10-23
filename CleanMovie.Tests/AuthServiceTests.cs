using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;
using CleanMovie.Service.Authentication;

namespace CleanMovie.Tests;

public class AuthServiceTests
{
    private const string JwtKey = "test-signing-key-that-is-at-least-32-bytes-long";
    private const string JwtIssuer = "CleanMovieAPI";
    private const string JwtAudience = "CleanMovieClient";

    private static MovieDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MovieDbContext(options);
    }

    private static AuthService CreateAuthService(MovieDbContext db)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = JwtKey,
                ["Jwt:Issuer"] = JwtIssuer,
                ["Jwt:Audience"] = JwtAudience
            })
            .Build();

        return new AuthService(new UserRepository(db), new JwtTokenService(configuration));
    }

    [Fact]
    public async Task Register_StoresUserWithHashedPasswordAndDefaultRole()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);

        await auth.RegisterAsync("alice", "Secret123!");

        var user = Assert.Single(db.Users);
        Assert.Equal("alice", user.Username);
        Assert.Equal("User", user.Role);
        Assert.NotEqual("Secret123!", user.PasswordHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("Secret123!", user.PasswordHash));
    }

    [Fact]
    public async Task Register_WithRole_StoresRole()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);

        await auth.RegisterAsync("admin", "Secret123!", "Admin");

        Assert.Equal("Admin", Assert.Single(db.Users).Role);
    }

    [Fact]
    public async Task Register_DuplicateUsername_ThrowsAndKeepsOriginalUser()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);
        await auth.RegisterAsync("alice", "First123!");

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => auth.RegisterAsync("alice", "Second123!"));

        Assert.Equal("Username already exists.", ex.Message);
        var user = Assert.Single(db.Users);
        Assert.True(BCrypt.Net.BCrypt.Verify("First123!", user.PasswordHash));
    }

    [Fact]
    public async Task Login_WithCorrectPassword_ReturnsValidTokenWithClaims()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);
        await auth.RegisterAsync("admin", "Secret123!", "Admin");

        var token = await auth.LoginAsync("admin", "Secret123!");

        Assert.NotNull(token);

        // Validerar signatur, utfärdare, mottagare och giltighetstid på samma sätt som API:t
        var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidIssuer = JwtIssuer,
            ValidAudience = JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey))
        }, out _);

        Assert.Equal("admin", principal.FindFirstValue(ClaimTypes.Name));
        Assert.True(principal.IsInRole("Admin"));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsNull()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);
        await auth.RegisterAsync("alice", "Secret123!");

        Assert.Null(await auth.LoginAsync("alice", "WrongPassword"));
    }

    [Fact]
    public async Task Login_WithUnknownUser_ReturnsNull()
    {
        await using var db = CreateContext();
        var auth = CreateAuthService(db);

        Assert.Null(await auth.LoginAsync("nobody", "Secret123!"));
    }
}
