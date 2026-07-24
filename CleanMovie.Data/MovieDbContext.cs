using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Entities;
using CleanMovie.Core.Interfaces;

namespace CleanMovie.Data;

public class MovieDbContext : DbContext, IMovieDbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public Microsoft.EntityFrameworkCore.DbSet<Movie> Movies => Set<Movie>();
    public Microsoft.EntityFrameworkCore.DbSet<MovieDetails> MovieDetails => Set<MovieDetails>();
    public Microsoft.EntityFrameworkCore.DbSet<Actor> Actors => Set<Actor>();
    public Microsoft.EntityFrameworkCore.DbSet<MovieActor> MovieActors => Set<MovieActor>();
    public Microsoft.EntityFrameworkCore.DbSet<Review> Reviews => Set<Review>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Details)
            .WithOne(d => d.Movie)
           .HasForeignKey<MovieDetails>(d => d.MovieId);

        modelBuilder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(ma => ma.ActorId);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId);
    }
}
