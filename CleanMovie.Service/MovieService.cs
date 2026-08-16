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

    public async Task UpdateAsync(int id, MovieDto dto)
    {
        // 1. Hämta filmen från DB (Viktigt: Repositoryt måste köra .Include(m => m.MovieActors))
        var movie = await _unitOfWork.Movies.GetAsync(id);

        if (movie is null)
            throw new KeyNotFoundException($"Movie {id} was not found.");

        // 2. Uppdatera filmens grunddata
        movie.Title = dto.Title;
        movie.Year = dto.Year;
        movie.Genre = dto.Genre;
        movie.Duration = dto.Duration;

        if (movie.MovieActors is null)
        {
            movie.MovieActors = new List<MovieActor>();
        }

        // 3. FIXEN: Istället för att köra .Clear(), lägger vi bara till de nya som saknas
        if (dto.Actors != null)
        {
            foreach (var actorDto in dto.Actors)
            {
                // Kontrollera om denna skådespelare REDAN är kopplad till filmen
                bool alreadyExists = movie.MovieActors.Any(ma => ma.ActorId == actorDto.Id);

                // Om den inte finns i databasens lista sedan tidigare, lägg till den nu
                if (!alreadyExists)
                {
                    movie.MovieActors.Add(new MovieActor
                    {
                        MovieId = id,
                        ActorId = actorDto.Id
                    });
                }
            }
        }

        // 4. Spara via Unit of Work
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