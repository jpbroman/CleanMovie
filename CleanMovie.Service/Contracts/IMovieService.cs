using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Core.Entities;

namespace CleanMovie.Core.DomainContracts;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllAsync(QueryParameters qp);
    Task<MovieDto?> GetAsync(int id);

    Task<MovieDto> CreateAsync(CreateMovieDto dto);
    Task UpdateAsync(int id, MovieDto dto);
    Task DeleteAsync(int id);
}
