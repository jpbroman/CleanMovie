using CleanMovie.Core.Entities;
using CleanMovie.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanMovie.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

        await context.Database.MigrateAsync();

        if (!await context.Movies.AnyAsync())
        {
            context.Movies.Add(new Movie
            {
                Title = "The Matrix",
                Year = 1999,
                Genre = "Sci-Fi",
                Duration = 136
            });

            await context.SaveChangesAsync();
        }
    }
}
