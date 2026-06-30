using CleanMovie.Service.Contracts;
namespace CleanMovie.Service;

public class ServiceManager : IServiceManager
{
    public ServiceManager(
        IMovieService movies,
        IActorService actors,
        IReviewService reviews,
        IMovieDetailService movieDetails)
    {
        Movies = movies;
        Actors = actors;
        Reviews = reviews;
        MovieDetails = movieDetails;
    }

    public IMovieService Movies { get; }

    public IActorService Actors { get; }

    public IReviewService Reviews { get; }

    public IMovieDetailService MovieDetails { get; }
}