using CleanMovie.Core.Entities;

namespace CleanMovie.Core.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}

