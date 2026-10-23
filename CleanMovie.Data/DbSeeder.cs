using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;

namespace CleanMovie.Data;

public static class DbSeeder
{
    public static void Seed(MovieDbContext context)
    {
        // 1. OM DET REDAN FINNS DATA, TÖM ALLT FÖR EN FRÄSCH OMSTART
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
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence;");
        }

        // 3. SKAPA SKÅDESPELARE (Objekten sparas i minnet och tilldelas ID automatiskt av EF)
        var actor1 = new Actor { Name = "Keanu Reeves", BirthDate = "1964-09-02" };
        var actor2 = new Actor { Name = "Laurence Fishburne", BirthDate = "1961-07-30" };
        var actor3 = new Actor { Name = "Sigourney Weaver", BirthDate = "1949-10-08" };
        var actor4 = new Actor { Name = "Morgan Freeman", BirthDate = "1937-06-01" };
        var actor5 = new Actor { Name = "Tim Robbins", BirthDate = "1958-10-16" };
        var actor6 = new Actor { Name = "Leonardo DiCaprio", BirthDate = "1974-11-11" };
        var actor7 = new Actor { Name = "Joseph Gordon-Levitt", BirthDate = "1981-02-17" };
        var actor8 = new Actor { Name = "Peter Stormare", BirthDate = "1953-08-27" };
        var actor9 = new Actor { Name = "Chuck Norris", BirthDate = "1940-03-10" };
        var actor10 = new Actor { Name = "Jennifer Aniston", BirthDate = "1969-02-11" };
        var actor11 = new Actor { Name = "Chevy Chase", BirthDate = "1943-10-08" };
        var actor12 = new Actor { Name = "Jennifer Connelly", BirthDate = "1970-12-12" };
        var actor13 = new Actor { Name = "Keira Knightley", BirthDate = "1985-03-26" };
        var actor14 = new Actor { Name = "Stellan Skarsgård", BirthDate = "1951-06-13" };

        context.Actors.AddRange(actor1, actor2, actor3, actor4, actor5, actor6, actor7,
            actor8, actor9, actor10, actor11, actor12, actor13, actor14);
        

        // 4. SKAPA FILMERNA OCH BYGG RELATIONERNA DIREKT
        var movies = new List<Movie>
        {
            new Movie
            {
                Title = "The Matrix",
                Year = 1999,
                Genre = "Action / Sci-Fi",
                Duration = 136,
                Details = new MovieDetails
                {
                    Synopsis = "A computer hacker learns from mysterious rebels about the true nature of his reality.",
                    Language = "English",
                    Budget = 63000000
                },
                Reviews = new List<Review>
                {
                    new Review { Comment = "An absolute masterpiece of modern cinema!", Rating = 5, Reviewer = "Peter" },
                    new Review { Comment = "Great special effects for its time.", Rating = 4, Reviewer = "Anna" }
                },
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
                Genre = "Horror / Sci-Fi",
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
                Genre = "Action / Thriller",
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
                    new MovieActor { Actor = actor1 } 
                }
            },
            // NY FILMER FÖR ATT REVOLVERA DE SKÅDESPELARE DU SKAPADE:
            new Movie
            {
                Title = "National Lampoon's Christmas Vacation",
                Year = 1989,
                Genre = "Komedi",
                Duration = 97,
                Details = new MovieDetails
                {
                    Synopsis = "The Griswold family's plans for a big family Christmas predictably turn into a big disaster.",
                    Language = "English",
                    Budget = 25000000
                },
                MovieActors = new List<MovieActor> { new MovieActor { Actor = actor11 } }
            },
            new Movie
            {
                Title = "Good Will Hunting",
                Year = 1997,
                Genre = "Drama",
                Duration = 126,
                Details = new MovieDetails
                {
                    Synopsis = "Will Hunting, a janitor at M.I.T., has a gift for mathematics, but needs help from a psychologist to find direction in his life.",
                    Language = "English",
                    Budget = 10000000
                },
                MovieActors = new List<MovieActor> { new MovieActor { Actor = actor14 } } // Stellan Skarsgård
            },
            new Movie
            {
                Title = "A Beautiful Mind",
                Year = 2001,
                Genre = "Drama / Biografi",
                Duration = 135,
                Details = new MovieDetails
                {
                    Synopsis = "After John Nash, a brilliant but asocial mathematician, accepts secret work in cryptography, his life takes a turn for the nightmarish.",
                    Language = "English",
                    Budget = 58000000
                },
                MovieActors = new List<MovieActor> { new MovieActor { Actor = actor12 } } // Jennifer Connelly
            },
            new Movie
            {
                Title = "Pirates of the Caribbean: The Curse of the Black Pearl",
                Year = 2003,
                Genre = "Action / Äventyr",
                Duration = 143,
                Details = new MovieDetails
                {
                    Synopsis = "Blacksmith Will Turner teams up with eccentric pirate 'Captain' Jack Sparrow to save his love from Jack's former mutinous cohorts.",
                    Language = "English",
                    Budget = 140000000
                },
                MovieActors = new List<MovieActor> { new MovieActor { Actor = actor13 } } // Keira Knightley
            },
            new Movie
            {
                Title = "Fargo",
                Year = 1996,
                Genre = "Thriller / Crime / Komedi",
                Duration = 98,
                Details = new MovieDetails
                {
                    Synopsis = "Jerry Lundegaard's inept crime falls apart due to his and his henchmen's bungling and the persistent police work of pregnant Marge Gunderson.",
                    Language = "English",
                    Budget = 7000000
                },
                MovieActors = new List<MovieActor> { new MovieActor { Actor = actor8 } } // Peter Stormare
            }
        };

        // 5. SPARA ALLT I DATABASEN VIA EN ENDA TRANSAKTION
        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}
