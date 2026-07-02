using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Core.Entities;

namespace CleanMovie.Core.DomainContracts;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync(QueryParameters qp);
    Task<MovieDto?> GetAsync(int id);

    Task<Movie> CreateAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int id);
}
