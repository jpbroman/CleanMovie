using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;
using CleanMovie.Service;

namespace CleanMovie.Tests;

// Varje test använder separata contexts för seed, uppdatering och kontroll,
// precis som separata HTTP-anrop mot API:t.
public class MovieServiceUpdateTests
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    private MovieDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;

        return new MovieDbContext(options);
    }

    private static MovieService CreateMovieService(MovieDbContext db)
    {
        var unitOfWork = new UnitOfWork(
            db,
            new MovieRepository(db),
            new ReviewRepository(db),
            new ActorRepository(db),
            new MovieDetailsRepository(db));

        return new MovieService(unitOfWork);
    }

    private async Task UpdateAsync(int id, MovieDto dto)
    {
        await using var db = CreateContext();
        await CreateMovieService(db).UpdateAsync(id, dto);
    }

    private async Task<Movie> LoadMovieAsync(int id)
    {
        await using var db = CreateContext();
        return await db.Movies
            .Include(m => m.Details)
            .Include(m => m.Reviews)
            .Include(m => m.MovieActors!)
                .ThenInclude(ma => ma.Actor)
            .SingleAsync(m => m.Id == id);
    }

    private async Task SeedAsync()
    {
        await using var db = CreateContext();

        var keanu = new Actor { Id = 1, Name = "Keanu Reeves", BirthDate = "1964-09-02" };
        var laurence = new Actor { Id = 2, Name = "Laurence Fishburne", BirthDate = "1961-07-30" };
        var carrie = new Actor { Id = 3, Name = "Carrie-Anne Moss", BirthDate = "1967-08-21" };
        db.Actors.AddRange(keanu, laurence, carrie);

        db.Movies.Add(new Movie
        {
            Id = 1,
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136,
            Details = new MovieDetails { Synopsis = "Red pill or blue pill.", Language = "English", Budget = 63000000 },
            Reviews = [new Review { Reviewer = "Anna", Comment = "Classic", Rating = 5 }],
            MovieActors = [new MovieActor { Actor = keanu }]
        });

        await db.SaveChangesAsync();
    }

    private static MovieDto Dto(params int[] actorIds) => new(
        1,
        "The Matrix",
        1999,
        "Sci-Fi",
        136,
        Actors: actorIds.Select(id => new ActorDto(id, $"Actor {id}", "")).ToList());

    [Fact]
    public async Task UpdatesTitleYearGenreAndDuration()
    {
        await SeedAsync();

        await UpdateAsync(1, new MovieDto(1, "The Matrix Reloaded", 2003, "Action", 138));

        var movie = await LoadMovieAsync(1);
        Assert.Equal("The Matrix Reloaded", movie.Title);
        Assert.Equal(2003, movie.Year);
        Assert.Equal("Action", movie.Genre);
        Assert.Equal(138, movie.Duration);
    }

    [Fact]
    public async Task UnknownMovie_ThrowsKeyNotFoundException()
    {
        await SeedAsync();

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => UpdateAsync(99, new MovieDto(99, "Missing", 2000, "Drama", 90)));

        Assert.Equal("Movie 99 was not found.", ex.Message);
    }

    [Fact]
    public async Task AddsNewActorsToMovie()
    {
        await SeedAsync();

        await UpdateAsync(1, Dto(1, 2, 3));

        var movie = await LoadMovieAsync(1);
        Assert.Equal([1, 2, 3], movie.MovieActors!.Select(ma => ma.ActorId).Order());
    }

    [Fact]
    public async Task ExistingActor_IsNotAddedTwice()
    {
        await SeedAsync();

        await UpdateAsync(1, Dto(1, 2));
        await UpdateAsync(1, Dto(1, 2));

        var movie = await LoadMovieAsync(1);
        Assert.Equal([1, 2], movie.MovieActors!.Select(ma => ma.ActorId).Order());
    }

    [Fact]
    public async Task DuplicateActorInDto_IsAddedOnce()
    {
        await SeedAsync();

        await UpdateAsync(1, Dto(2, 2));

        var movie = await LoadMovieAsync(1);
        Assert.Equal([1, 2], movie.MovieActors!.Select(ma => ma.ActorId).Order());
    }

    [Fact]
    public async Task ActorsMissingFromDto_AreKept()
    {
        await SeedAsync();

        await UpdateAsync(1, Dto(2));

        var movie = await LoadMovieAsync(1);
        Assert.Equal([1, 2], movie.MovieActors!.Select(ma => ma.ActorId).Order());
    }

    [Fact]
    public async Task NullActors_KeepsExistingActors()
    {
        await SeedAsync();

        await UpdateAsync(1, new MovieDto(1, "The Matrix", 1999, "Sci-Fi", 136, Actors: null));

        var movie = await LoadMovieAsync(1);
        Assert.Equal([1], movie.MovieActors!.Select(ma => ma.ActorId));
    }

    [Fact]
    public async Task DoesNotChangeDetailsOrReviews()
    {
        await SeedAsync();

        await UpdateAsync(1, new MovieDto(1, "The Matrix", 1999, "Sci-Fi", 136));

        var movie = await LoadMovieAsync(1);
        Assert.NotNull(movie.Details);
        Assert.Equal("Red pill or blue pill.", movie.Details.Synopsis);
        var review = Assert.Single(movie.Reviews!);
        Assert.Equal("Classic", review.Comment);
    }
}
