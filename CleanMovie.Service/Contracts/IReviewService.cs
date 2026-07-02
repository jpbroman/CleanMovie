using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;

namespace CleanMovie.Service.Contracts;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<IEnumerable<Review>> GetByMovieAsync(int movieId);
    Task<Review?> GetAsync(int id);
    Task<Review> CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(int id);
}
