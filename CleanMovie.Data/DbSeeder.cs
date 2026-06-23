using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Models;

namespace CleanMovie.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(MovieDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Movies.AnyAsync())
            return;

        // Movies
        var movies = new List<Movie>
        {
            new() { Id = 1, Title = "The Matrix", Year = 1999, Genre = "Sci-Fi", Duration = 136 },
            new() { Id = 2, Title = "Inception", Year = 2010, Genre = "Sci-Fi", Duration = 148 },
            new() { Id = 3, Title = "The Godfather", Year = 1972, Genre = "Crime", Duration = 175 },
            new() { Id = 4, Title = "Pulp Fiction", Year = 1994, Genre = "Crime", Duration = 154 },
            new() { Id = 5, Title = "Interstellar", Year = 2014, Genre = "Sci-Fi", Duration = 169 }
        };

        context.Movies.AddRange(movies);

        var details = new List<MovieDetails>
        {
            new() { MovieId = 1, Synopsis = "A hacker discovers reality is a simulation." },
            new() { MovieId = 2, Synopsis = "A thief who steals corporate secrets through dream-sharing technology." },
            new() { MovieId = 3, Synopsis = "The aging patriarch of an organized crime dynasty transfers control to his reluctant son." },
            new() { MovieId = 4, Synopsis = "The lives of two mob hitmen, a boxer, and others intertwine in four tales of violence and redemption." },
            new() { MovieId = 5, Synopsis = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival." }
        };
        context.MovieDetails.AddRange(details);

        // Actors
        var actors = new List<Actor>
        {
            new() { Id = 1, Name = "Keanu Reeves", BirthDate = new DateTime(1964, 9, 2) },
            new() { Id = 2, Name = "Laurence Fishburne", BirthDate = new DateTime(1961, 7, 30) },
            new() { Id = 3, Name = "Leonardo DiCaprio", BirthDate = new DateTime(1974, 11, 11) },
            new() { Id = 4, Name = "Joseph Gordon-Levitt", BirthDate = new DateTime(1981, 2, 17) },
            new() { Id = 5, Name = "Marlon Brando", BirthDate = new DateTime(1924, 4, 3) },
            new() { Id = 6, Name = "Al Pacino", BirthDate = new DateTime(1940, 4, 25) },
            new() { Id = 7, Name = "John Travolta", BirthDate = new DateTime(1954, 2, 18) },
            new() { Id = 8, Name = "Matthew McConaughey", BirthDate = new DateTime(1969, 11, 4) }
        };

        context.Actors.AddRange(actors);

        await context.SaveChangesAsync(); // IMPORTANT: generates IDs if needed

        var movieActors = new List<MovieActor>
        {
            new() { MovieId = 1, ActorId = 1 },
            new() { MovieId = 1, ActorId = 2 },

            new() { MovieId = 2, ActorId = 3 },
            new() { MovieId = 2, ActorId = 4 },

            new() { MovieId = 3, ActorId = 5 },
            new() { MovieId = 3, ActorId = 6 },

            new() { MovieId = 4, ActorId = 7 },

            new() { MovieId = 5, ActorId = 8 }
        };

        context.MovieActors.AddRange(movieActors);

        var reviews = new List<Review>
        {
            new() { MovieId = 1, Reviewer = "Alice", Comment = "Classic sci-fi masterpiece", Rating = 10 },
            new() { MovieId = 1, Reviewer = "Bob", Comment = "Still amazing today", Rating = 9 },

            new() { MovieId = 2, Reviewer = "Charlie", Comment = "Mind bending!", Rating = 9 },

            new() { MovieId = 3, Reviewer = "Diana", Comment = "Best mafia movie ever", Rating = 10 },

            new() { MovieId = 4, Reviewer = "Edward", Comment = "Quentin at his best", Rating = 9 },

            new() { MovieId = 5, Reviewer = "Fiona", Comment = "Beautiful and emotional", Rating = 10 }
        };

        context.Reviews.AddRange(reviews);

        await context.SaveChangesAsync();
    }
}
