using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanMovie.Data.Repositories;


public class MovieDetailsRepository : IMovieDetailsRepository
{
    private readonly IMovieDbContext _context;

    public MovieDetailsRepository(IMovieDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieDetails>> GetAllAsync()
    {
        return await _context.MovieDetails
            .Include(md => md.Movie)
            .ToListAsync();
    }

    public async Task<MovieDetails?> GetAsync(int id)
    {
        return await _context.MovieDetails
            .Include(md => md.Movie)
            .FirstOrDefaultAsync(md => md.Id == id);
    }

    public async Task AddAsync(MovieDetails movieDetails)
    {
        await _context.MovieDetails.AddAsync(movieDetails);
    }

    public void Update(MovieDetails movieDetails)
    {
        _context.MovieDetails.Update(movieDetails);
    }

    public void Delete(MovieDetails movieDetails)
    {
        _context.MovieDetails.Remove(movieDetails);
    }
}
