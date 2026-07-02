using CleanMovie.Core.Interfaces;
namespace CleanMovie.Service.Contracts;

using CleanMovie.Core.Entities;

public interface IMovieDetailService
{
    Task<IEnumerable<MovieDetails>> GetAllAsync();
    Task<MovieDetails?> GetAsync(int id);
    Task<MovieDetails> CreateAsync(MovieDetails details);
    Task UpdateAsync(MovieDetails details);
    Task DeleteAsync(int id);
}
