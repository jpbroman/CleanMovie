using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Core.DomainContracts;

namespace CleanMovie.Service;


public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;

    public MovieService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static MovieDto Map(Movie movie)
    {
        return new MovieDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Genre,
            movie.Duration,
            movie.Details is not null
                ? new MovieDetailsDto(
                    movie.Details.MovieId,
                    movie.Details.Synopsis,
                    movie.Details.Language,
                    movie.Details.Budget)
                : null,
                movie.MovieActors?.Select(ma => new ActorDto(
                    ma.Actor.Id,
                    ma.Actor.Name,
                    ma.Actor.BirthDate)),
                movie.Reviews?.Select(r => new ReviewDto(
                    r.Id,
                    r.Reviewer,
                    r.Comment,
                    r.Rating)));
    }

    public async Task<List<MovieDto>> GetAllAsync(QueryParameters qp)
    {
        var movies = await _unitOfWork.Movies.GetAllAsync(qp);

        return movies.Select(Map).ToList();
    }

    public async Task<MovieDto?> GetAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        return movie is null
            ? null
            : Map(movie);
    }

    public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Year = dto.Year,
            Genre = dto.Genre,
            Duration = dto.Duration
        };

        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return Map(movie);
    }
    public async Task UpdateAsync(int id, CreateMovieDto dto)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        if (movie is null)
            throw new KeyNotFoundException($"Movie {id} was not found.");

        // Update movie
        movie.Title = dto.Title;
        movie.Year = dto.Year;
        movie.Genre = dto.Genre;
        movie.Duration = dto.Duration;

        _unitOfWork.Movies.Update(movie);

        await _unitOfWork.CompleteAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        if (movie is null)
            return;

        _unitOfWork.Movies.Remove(movie);

        await _unitOfWork.CompleteAsync();
    }
}