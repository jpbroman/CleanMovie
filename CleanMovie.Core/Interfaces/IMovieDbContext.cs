using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Models;

namespace CleanMovie.Core.Interfaces;

public interface IMovieDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Actor> Actors { get; }
    DbSet<MovieDetails> MovieDetails { get; }
    DbSet<Review> Reviews { get; }
    DbSet<MovieActor> MovieActors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

