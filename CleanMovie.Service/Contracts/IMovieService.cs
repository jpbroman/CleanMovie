using CleanMovie.Core.Entities;
namespace CleanMovie.Service.Contracts;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetAsync(int id);
    Task<Movie> CreateAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int id);

}