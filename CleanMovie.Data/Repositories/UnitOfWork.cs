using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Interfaces;
using SQLitePCL;

namespace CleanMovie.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMovieDbContext _context;

    public IMovieRepository Movies { get; }
    public IReviewRepository Reviews { get; }
    public IActorRepository Actors { get; }
    public IMovieDetailsRepository MovieDetails { get; }

    public UnitOfWork(
        IMovieDbContext context,
        IMovieRepository movies,
        IReviewRepository reviews,
        IActorRepository actors,
        IMovieDetailsRepository movieDetails)
    {
        _context = context;

        Movies = movies;
        Reviews = reviews;
        Actors = actors;
        MovieDetails = movieDetails;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}