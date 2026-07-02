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
            movie.Details is null
                ? null
                : new MovieDetailsDto(
                    movie.Details.Synopsis,
                    movie.Details.Language,
                    movie.Details.Budget),
            movie.MovieActors.Select(ma => new ActorDto(
                ma.Actor.Id,
                ma.Actor.Name,
                ma.Actor.BirthDate)),
            movie.Reviews.Select(r => new ReviewDto(
                r.Id,
                r.Reviewer,
                r.Comment,
                r.Rating))
        );
    }

    public async Task<IEnumerable<MovieDto>> GetAllAsync(QueryParameters qp)
    {
        var movies = await _unitOfWork.Movies.GetAllAsync(qp);

        return movies.Select(Map);
    }

    public async Task<MovieDto?> GetAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        return movie is null
            ? null
            : Map(movie);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return movie;
    }

    public async Task UpdateAsync(Movie movie)
    {
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