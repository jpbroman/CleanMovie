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

    public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Year = dto.Year,
            Genre = dto.Genre,
            Duration = dto.Duration
        };

        if (dto.Details is not null)
        {
            movie.Details = new MovieDetails
            {
                Synopsis = dto.Details.Synopsis,
                Language = dto.Details.Language,
                Budget = dto.Details.Budget
            };
        }

        foreach (var actorDto in dto.Actors)
        {
            var actor = new Actor
            {
                Name = actorDto.Name,
                BirthDate = actorDto.BirthDate
            };

            movie.MovieActors.Add(new MovieActor
            {
                Actor = actor
            });
        }

        foreach (var reviewDto in dto.Reviews)
        {
            movie.Reviews.Add(new Review
            {
                Reviewer = reviewDto.Reviewer,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating
            });
        }

        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return Map(movie);
    }
/*
    public async Task<Movie> CreateAsync(Movie movie)
    {
        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return movie;
    }
*/
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

        // Update details
        if (dto.Details is not null)
        {
            if (movie.Details is null)
            {
                movie.Details = new MovieDetails();
            }

            movie.Details.Synopsis = dto.Details.Synopsis;
            movie.Details.Language = dto.Details.Language;
            movie.Details.Budget = dto.Details.Budget;
        }

        // Replace actors
        movie.MovieActors.Clear();

        foreach (var actorDto in dto.Actors)
        {
            movie.MovieActors.Add(new MovieActor
            {
                Actor = new Actor
                {
                    Name = actorDto.Name,
                    BirthDate = actorDto.BirthDate
                }
            });
        }

        // Replace reviews
        movie.Reviews.Clear();

        foreach (var reviewDto in dto.Reviews)
        {
            movie.Reviews.Add(new Review
            {
                Reviewer = reviewDto.Reviewer,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating
            });
        }

        _unitOfWork.Movies.Update(movie);

        await _unitOfWork.CompleteAsync();
    }
/*    public async Task UpdateAsync(Movie movie)
    {
        _unitOfWork.Movies.Update(movie);
        await _unitOfWork.CompleteAsync();
    }
*/
    public async Task DeleteAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        if (movie is null)
            return;

        _unitOfWork.Movies.Remove(movie);

        await _unitOfWork.CompleteAsync();
    }
}