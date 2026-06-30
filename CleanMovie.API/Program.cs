using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.DomainContracts;
using CleanMovie.Data;
using CleanMovie.Service;using CleanMovie.Service.Contracts;
using CleanMovie.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' was not found.");

// DbContext
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// DbContext interface
builder.Services.AddScoped<IMovieDbContext>(
    provider => provider.GetRequiredService<MovieDbContext>());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IMovieDetailService, MovieDetailService>();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
// Repositories
// builder.Services.AddScoped<IMovieRepository, MovieRepository>();
// builder.Services.AddScoped<IActorRepository, ActorRepository>();
// builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
// builder.Services.AddScoped<IMovieDetailsRepository, MovieDetailsRepository>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Controllers + JSON serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

    await context.Database.MigrateAsync();

    // Optional seed data
    if (!await context.Movies.AnyAsync())
    {
        context.Movies.Add(new CleanMovie.Core.Entities.Movie
        {
            Title = "The Matrix",
            Year = 1999,
            Genre = "Sci-Fi",
            Duration = 136
        });

        await context.SaveChangesAsync();
    }
}

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
