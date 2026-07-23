namespace CleanMovie.Core.Entities;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    // BCrypt hashed password
    public string PasswordHash { get; set; } = string.Empty;

    // User or Admin
    public string Role { get; set; } = "User";
}
