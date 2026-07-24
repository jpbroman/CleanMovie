using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Interfaces;
using CleanMovie.Data.Services;
using CleanMovie.Service;
using CleanMovie.Service.Authentication;
using CleanMovie.Service.Contracts;
using CleanMovie.Services;

namespace CleanMovie.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IMovieDetailService, MovieDetailService>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
