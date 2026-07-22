using CleanMovie.Core.Entities;
using Xunit;

namespace CleanMovie.Tests;

public class MovieTests
{
    [Fact]
    public void CanCreateMovie()
    {
        var movie = new Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        };

        Assert.Equal("The Matrix", movie.Title);
    }
}
