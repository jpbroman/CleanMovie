using CleanMovie.Core.Entities;
namespace CleanMovie.Core.DomainContracts;

public interface IMovieRepository
{
    // I IMovieRepository.cs
    Task<List<Movie>> GetAllAsync(QueryParameters queryParameters);

//    Task<IEnumerable<Movie>> GetAllAsync(QueryParameters qp);
    Task<Movie?> GetAsync(int id);
    Task<bool> AnyAsync(int id);
    Task AddAsync(Movie movie);
    void Update(Movie movie);
    void Remove(Movie movie);
}
