using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleanMovie.API.Controllers;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;
using CleanMovie.Service;

namespace CleanMovie.Tests;

public class ReportsTests
{
    private static MovieDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MovieDbContext(options);
    }

    private static Movie AddMovie(MovieDbContext db, string genre, params int[] ratings)
    {
        var movie = new Movie
        {
            Title = $"{genre} movie",
            Year = 2000,
            Genre = genre,
            Duration = 120,
            Reviews = ratings
                .Select(r => new Review { Reviewer = "Tester", Comment = "Ok", Rating = r })
                .ToList()
        };

        db.Movies.Add(movie);
        return movie;
    }

    private static void Cast(MovieDbContext db, Actor actor, params Movie[] movies)
    {
        foreach (var movie in movies)
        {
            db.MovieActors.Add(new MovieActor { Movie = movie, Actor = actor });
        }
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

    [Fact]
    public async Task EmptyDatabase_ReturnsEmptyLists()
    {
        await using var db = CreateContext();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        Assert.Empty(report.TopGenresByRating);
        Assert.Empty(report.MostActiveActors);
    }

    [Fact]
    public async Task TopGenres_AveragesAllReviewsPerGenre_RoundedAndSortedDescending()
    {
        await using var db = CreateContext();
        AddMovie(db, "Sci-Fi", 5, 4);
        AddMovie(db, "Sci-Fi", 3);
        AddMovie(db, "Drama", 2, 3);
        AddMovie(db, "Comedy", 4, 5, 5);
        await db.SaveChangesAsync();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        Assert.Equal(
            [
                new GenreRatingDto("Comedy", 4.7),
                new GenreRatingDto("Sci-Fi", 4.0),
                new GenreRatingDto("Drama", 2.5)
            ],
            report.TopGenresByRating);
    }

    [Fact]
    public async Task TopGenres_ExcludesMoviesWithoutReviews()
    {
        await using var db = CreateContext();
        AddMovie(db, "Horror");
        AddMovie(db, "Drama", 4);
        await db.SaveChangesAsync();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        var genre = Assert.Single(report.TopGenresByRating);
        Assert.Equal("Drama", genre.Genre);
    }

    [Fact]
    public async Task TopGenres_ReturnsOnlyTheFiveHighestRated()
    {
        await using var db = CreateContext();
        AddMovie(db, "Horror", 1);
        AddMovie(db, "Drama", 2);
        AddMovie(db, "Western", 3);
        AddMovie(db, "Comedy", 4);
        AddMovie(db, "Sci-Fi", 5);
        AddMovie(db, "Action", 4, 5);
        await db.SaveChangesAsync();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        Assert.Equal(
            ["Sci-Fi", "Action", "Comedy", "Western", "Drama"],
            report.TopGenresByRating.Select(g => g.Genre));
    }

    [Fact]
    public async Task MostActiveActors_CountsMoviesPerActor_SortedDescending()
    {
        await using var db = CreateContext();
        var matrix = AddMovie(db, "Sci-Fi");
        var reloaded = AddMovie(db, "Sci-Fi");
        var johnWick = AddMovie(db, "Action");

        Cast(db, new Actor { Name = "Keanu Reeves" }, matrix, reloaded, johnWick);
        Cast(db, new Actor { Name = "Laurence Fishburne" }, matrix, reloaded);
        Cast(db, new Actor { Name = "Carrie-Anne Moss" }, matrix);
        await db.SaveChangesAsync();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        Assert.Equal(
            [
                new ActorActivityDto("Keanu Reeves", 3),
                new ActorActivityDto("Laurence Fishburne", 2),
                new ActorActivityDto("Carrie-Anne Moss", 1)
            ],
            report.MostActiveActors);
    }

    [Fact]
    public async Task MostActiveActors_ReturnsOnlyTheFiveMostActive()
    {
        await using var db = CreateContext();
        var movies = Enumerable.Range(0, 6).Select(_ => AddMovie(db, "Drama")).ToArray();

        // Actor i spelar i i+1 filmer, så "Actor 0" är minst aktiv
        for (var i = 0; i < 6; i++)
        {
            Cast(db, new Actor { Name = $"Actor {i}" }, movies.Take(i + 1).ToArray());
        }
        await db.SaveChangesAsync();

        var report = await new MovieRepository(db).GetDashboardStatisticsAsync();

        Assert.Equal(5, report.MostActiveActors.Count);
        Assert.DoesNotContain(report.MostActiveActors, a => a.ActorName == "Actor 0");
        Assert.Equal(new ActorActivityDto("Actor 5", 6), report.MostActiveActors[0]);
    }

    [Fact]
    public async Task MovieService_ReturnsStatisticsFromRepository()
    {
        await using var db = CreateContext();
        var movie = AddMovie(db, "Drama", 4);
        Cast(db, new Actor { Name = "Morgan Freeman" }, movie);
        await db.SaveChangesAsync();

        var report = await CreateMovieService(db).GetDashboardStatisticsAsync();

        Assert.Equal([new GenreRatingDto("Drama", 4.0)], report.TopGenresByRating);
        Assert.Equal([new ActorActivityDto("Morgan Freeman", 1)], report.MostActiveActors);
    }

    [Fact]
    public async Task ReportsController_GetReport_ReturnsOkWithDashboardReport()
    {
        await using var db = CreateContext();
        AddMovie(db, "Comedy", 5);
        await db.SaveChangesAsync();
        var controller = new ReportsController(CreateMovieService(db));

        var result = await controller.GetReport();

        var ok = Assert.IsType<OkObjectResult>(result);
        var report = Assert.IsType<DashboardReportDto>(ok.Value);
        Assert.Equal([new GenreRatingDto("Comedy", 5.0)], report.TopGenresByRating);
        Assert.Empty(report.MostActiveActors);
    }
}
