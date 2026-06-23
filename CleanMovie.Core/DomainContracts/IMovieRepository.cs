using CleanMovie.Core.Models;
namespace CleanMovie.Core.DomainContracts;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetAsync(int id);
    Task<bool> AnyAsync(int id);
    Task AddAsync(Movie movie);
    void Update(Movie movie);
    void Remove(Movie movie);
}
