using CleanMovie.Core.Models;

namespace CleanMovie.Core.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<Review?> GetAsync(int id);
    Task<IEnumerable<Review>> GetByMovieIdAsync(int movieId);
    Task AddAsync(Review review);
    void Update(Review review);
    void Delete(Review review);
}