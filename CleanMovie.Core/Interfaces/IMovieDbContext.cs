using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;

namespace CleanMovie.Core.Interfaces;

public interface IMovieDbContext
{
    // Ändra till att bara ha { get; } eftersom klassen använder uttrycksfyllda egenskaper
    DbSet<Movie> Movies { get; }
    DbSet<MovieDetails> MovieDetails { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Actor> Actors { get; }
    DbSet<MovieActor> MovieActors { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}

