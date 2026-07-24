using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Interfaces;
using CleanMovie.Data.Repositories;

namespace CleanMovie.API.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IMovieDetailsRepository, MovieDetailsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
