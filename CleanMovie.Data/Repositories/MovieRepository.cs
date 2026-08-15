using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Entities.DTOs;

namespace CleanMovie.Data.Repositories;

public class MovieRepository : IMovieRepository
{
    private IMovieDbContext _context;
    
    public MovieRepository(IMovieDbContext context)
    {
        _context = context;
    }

// I MovieRepository.cs
    public async Task<List<Movie>> GetAllAsync(QueryParameters queryParameters)
    {
        return await _context.Movies
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();
    }

    // public async Task<Movie?> GetAsync(int id)
    // {
    //     return await _context.Movies
    //         .FirstOrDefaultAsync(m => m.Id == id);
    // }

// MovieRepository.cs
    public async Task<Movie?> GetAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.Details)
            .Include(m => m.Reviews)
            .Include(m => m.MovieActors!)
                .ThenInclude(ma => ma.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> AnyAsync(int id)
    {
        return (await _context.Movies.FindAsync(id) != null);
    }

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
    }

    public void Update(Movie movie)
    {
        _context.Movies.Update(movie);
    }
    
    public void Remove(Movie movie)
    {
        _context.Movies.Remove(movie);
    }
}
