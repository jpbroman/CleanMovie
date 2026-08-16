using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;
using CleanMovie.Data;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;


public class MovieDbTests
{
    [Fact]
    public async Task CanAddMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        db.Movies.Add(new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        });

        await db.SaveChangesAsync();

        Assert.Equal(1, await db.Movies.CountAsync());
    }

    [Fact]
    public async Task CanRetrieveMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        db.Movies.Add(movie);
        await db.SaveChangesAsync();

        var retrievedMovie = await db.Movies.FirstOrDefaultAsync();
        Assert.NotNull(retrievedMovie);
        Assert.Equal("The Matrix", retrievedMovie.Title);
    }

    [Fact]
    public async Task CanUpdateMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        db.Movies.Add(movie);
        await db.SaveChangesAsync();

        movie.Title = "Matrix";
        await db.SaveChangesAsync();

        var retrievedMovie = await db.Movies.FirstOrDefaultAsync();
        Assert.NotNull(retrievedMovie);
        Assert.Equal("Matrix", retrievedMovie.Title);
    }

    [Fact]
    public async Task CanDeleteMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        db.Movies.Add(movie);
        await db.SaveChangesAsync();

        db.Movies.Remove(movie);
        await db.SaveChangesAsync();

        Assert.Equal(0, await db.Movies.CountAsync());
    }

    [Fact]
    public async Task CanAddDetailsToMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        db.Movies.Add(movie);
        await db.SaveChangesAsync();

        var details = new MovieDetails
        {
            MovieId = movie.Id,
            Synopsis = "A computer hacker learns about the true nature of reality and his role in the war against its controllers.",
            Language = "English",
            Budget = 63000000
        };

        db.MovieDetails.Add(details);
        await db.SaveChangesAsync();

        var retrievedDetails = await db.MovieDetails.FirstOrDefaultAsync();
        Assert.NotNull(retrievedDetails);
        Assert.Equal("A computer hacker learns about the true nature of reality and his role in the war against its controllers.", retrievedDetails.Synopsis);
    }

    [Fact]
    public async Task CanAddActorToMovie()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        db.Movies.Add(movie);
        await db.SaveChangesAsync();

        var actor = new Actor
        {
            Name = "Keanu Reeves",
            BirthDate = new DateTime(1964, 9, 2).ToString(),
        };

        db.Actors.Add(actor);
        await db.SaveChangesAsync();

        var movieActor = new MovieActor
        {
            MovieId = movie.Id,
            ActorId = actor.Id
        };

        db.MovieActors.Add(movieActor);
        await db.SaveChangesAsync();

        var retrievedMovieActor = await db.MovieActors.FirstOrDefaultAsync();
        Assert.NotNull(retrievedMovieActor);
        Assert.Equal(movie.Id, retrievedMovieActor.MovieId);
        Assert.Equal(actor.Id, retrievedMovieActor.ActorId);
        Assert.Equal("1964-09-02", actor.BirthDate);
    }  
}