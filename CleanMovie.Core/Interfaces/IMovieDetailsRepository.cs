using CleanMovie.Core.Models;

namespace CleanMovie.Core.Interfaces;

public interface IMovieDetailsRepository
{
    Task<IEnumerable<MovieDetails>> GetAllAsync();
    Task<MovieDetails?> GetAsync(int id);
    Task AddAsync(MovieDetails movieDetails);
    void Update(MovieDetails movieDetails);
    void Delete(MovieDetails movieDetails);
}

