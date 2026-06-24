using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;

namespace CleanMovie.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly IMovieDbContext _context;

    public ReviewRepository(IMovieDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review?> GetAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Reviews
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public void Update(Review review)
    {
        _context.Reviews.Update(review);
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }
}
