using CleanMovie.Core.Interfaces;

namespace CleanMovie.Core.DomainContracts;

public interface IUnitOfWork
{
    IMovieRepository Movies { get; }
    IReviewRepository Reviews { get; }
    IActorRepository Actors { get; }
    IMovieDetailsRepository MovieDetails { get; }
    Task CompleteAsync();
}
