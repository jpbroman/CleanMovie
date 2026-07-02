using CleanMovie.Core.DomainContracts;

namespace CleanMovie.Service.Contracts;

public interface IServiceManager
{
    IMovieService Movies { get; }
    IActorService Actors { get; }
    IReviewService Reviews { get; }
    IMovieDetailService MovieDetails { get; }
}
