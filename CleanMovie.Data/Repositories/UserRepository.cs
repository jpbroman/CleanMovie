using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;

namespace CleanMovie.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMovieDbContext _context;

    public UserRepository(IMovieDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
