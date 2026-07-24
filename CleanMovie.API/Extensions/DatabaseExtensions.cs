using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Interfaces;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanMovie.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MovieDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMovieDbContext>(
            provider => provider.GetRequiredService<MovieDbContext>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
