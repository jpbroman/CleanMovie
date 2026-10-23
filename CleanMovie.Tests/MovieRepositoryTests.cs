using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;

namespace CleanMovie.Tests;

public class MovieRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_PagesInIdOrder()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new MovieDbContext(options);

        // Läggs till i omvänd ordning för att visa att sidorna inte beror på insättningsordningen
        foreach (var id in new[] { 5, 4, 3, 2, 1 })
        {
            db.Movies.Add(new Movie { Id = id, Title = $"Movie {id}", Year = 2000, Genre = "Drama", Duration = 120 });
        }
        await db.SaveChangesAsync();

        var repository = new MovieRepository(db);

        var firstPage = await repository.GetAllAsync(new QueryParameters { PageNumber = 1, PageSize = 2 });
        var secondPage = await repository.GetAllAsync(new QueryParameters { PageNumber = 2, PageSize = 2 });
        var lastPage = await repository.GetAllAsync(new QueryParameters { PageNumber = 3, PageSize = 2 });

        Assert.Equal([1, 2], firstPage.Select(m => m.Id));
        Assert.Equal([3, 4], secondPage.Select(m => m.Id));
        Assert.Equal([5], lastPage.Select(m => m.Id));
    }
}
