using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;

namespace CleanMovie.Data;

public static class DbSeeder
{
    public static void Seed(MovieDbContext context)
    {
        if (context.Movies.Any())
        {
            // Töm alla relaterade tabeller först (så att inga främmande nycklar kraschar)
            context.MovieActors.RemoveRange(context.MovieActors);
            context.Reviews.RemoveRange(context.Reviews);
            context.MovieDetails.RemoveRange(context.MovieDetails);
            context.Actors.RemoveRange(context.Actors);
            context.Movies.RemoveRange(context.Movies);
            context.SaveChanges();

            // 2. NOLLSTÄLL ID-RÄKNAREN I SQLITE:
            // Detta kommando nollställer autoincrement-räknaren för ALLA tabeller i SQLite
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence;");
        }

        context.Movies.RemoveRange(context.Movies);
        context.SaveChanges();

       // 1. Kontrollera om det redan finns filmer i databasen för att undvika dubbletter
        if (context.Movies.Any())
        {
            return; 
        }

        // 2. Skapa några fristående skådespelare som vi kan återanvända
        var actor1 = new Actor { Name = "Keanu Reeves", BirthDate = "1961-07-01" };
        var actor2 = new Actor { Name = "Laurence Fishburne", BirthDate = "1957-06-22" };
        var actor3 = new Actor { Name = "Sigourney Weaver", BirthDate = "1959-02-12" };
        var actor4 = new Actor { Name = "Morgan Freeman", BirthDate = "1951-11-13" };
        var actor5 = new Actor { Name = "Tim Robbins", BirthDate = "1999-05-11" };
        var actor6 = new Actor { Name = "Leonardo DiCaprio", BirthDate = "1972-09-18" };
        var actor7 = new Actor { Name = "Joseph Gordon-Levitt", BirthDate = "1967-06-01" };
        var actor8 = new Actor { Name = "Peter Broman", BirthDate = "1969-06-01" };
        var actor9 = new Actor { Name = "Chuck Norris", BirthDate = "1940-03-10" };
        var actor10 = new Actor { Name = "Jennifer Aniston", BirthDate = "1969-02-11" };
        var actor11 = new Actor { Name = "Chevy Chase", BirthDate = "1943-10-08" };
        var actor12 = new Actor { Name = "Jennifer Connely", BirthDate = "1970-12-12" };
        var actor13 = new Actor { Name = "Kiera Knightley", BirthDate = "1985-03-26" };
        var actor14 = new Actor { Name = "Stellan Skarsgård", BirthDate = "1951-06-13" };



        context.Actors.AddRange(actor1, actor2, actor3,actor4, actor5, actor6, actor7);
        

        // 3. Skapa filmerna och bygg relationerna direkt i objekten
        var movies = new List<Movie>
        {
            new Movie
            {
                Title = "The Matrix",
                Year = 1999,
                Genre = "Sci-Fi",
                Duration = 136,
                // Lägg till 1:1 relationen direkt
                Details = new MovieDetails
                {
                    Synopsis = "A computer hacker learns from mysterious rebels about the true nature of his reality.",
                    Language = "English",
                    Budget = 63000000
                },
                // Lägg till 1:N relationen (Reviews)
                Reviews = new List<Review>
                {
                    new Review { Comment = "An absolute masterpiece of modern cinema!", Rating = 5, Reviewer = "Peter" },
                    new Review { Comment = "Great special effects for its time.", Rating = 4, Reviewer = "Anna" }
                },
                // Lägg till N:M relationen (Kopplingstabellen MovieActors)
                MovieActors = new List<MovieActor>
                {
                    new MovieActor { Actor = actor1 },
                    new MovieActor { Actor = actor2 }
                }
            },
            new Movie
            {
                Title = "Alien",
                Year = 1979,
                Genre = "Horror",
                Duration = 117,
                Details = new MovieDetails
                {
                    Synopsis = "The crew of a commercial spacecraft encounters a deadly lifeform in deep space.",
                    Language = "English",
                    Budget = 11000000
                },
                Reviews = new List<Review>
                {
                    new Review { Comment = "Terrifying and beautiful.", Rating = 5, Reviewer = "Erik" }
                },
                MovieActors = new List<MovieActor>
                {
                    new MovieActor { Actor = actor3 }
                }
            },
            new Movie
            {
                Title = "The Shawshank Redemption",
                Year = 1994,
                Genre = "Drama",
                Duration = 142,
                Details = new MovieDetails
                {
                    Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    Language = "English",
                    Budget = 25000000
                },
                Reviews = new List<Review>
                {
                    new Review { Comment = "The highest rated movie for a reason. Flawless.", Rating = 5, Reviewer = "Johan" },
                    new Review { Comment = "Very emotional and inspiring story.", Rating = 5, Reviewer = "Sofia" }
                },
                MovieActors = new List<MovieActor>
                {
                    new MovieActor { Actor = actor4 },
                    new MovieActor { Actor = actor5 }
                }
            },
            new Movie
            {
                Title = "Inception",
                Year = 2010,
                Genre = "Action / Sci-Fi",
                Duration = 148,
                Details = new MovieDetails
                {
                    Synopsis = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                    Language = "English",
                    Budget = 160000000
                },
                Reviews = new List<Review>
                {
                    new Review { Comment = "Mind-bending plot and incredible soundtrack!", Rating = 5, Reviewer = "Lucas" },
                    new Review { Comment = "A bit confusing the first time, but amazing.", Rating = 4, Reviewer = "Emma" }
                },
                MovieActors = new List<MovieActor>
                {
                    new MovieActor { Actor = actor6 },
                    new MovieActor { Actor = actor7 }
                }
            },
            new Movie
            {
                Title = "John Wick",
                Year = 2014,
                Genre = "Action",
                Duration = 101,
                Details = new MovieDetails
                {
                    Synopsis = "An ex-hit-man comes out of retirement to track down the gangsters that killed his dog and took everything from him.",
                    Language = "English",
                    Budget = 20000000
                },
                Reviews = new List<Review>
                {
                    new Review { Comment = "Pure adrenaline from start to finish.", Rating = 4, Reviewer = "Marcus" }
                },
                MovieActors = new List<MovieActor>
                {
                    // Här återanvänder vi Keanu Reeves (actor1) från förra exemplet!
                    new MovieActor { Actor = actor1 } 
                }
            }

        };

        // 4. Spara allt i databasen
        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}
