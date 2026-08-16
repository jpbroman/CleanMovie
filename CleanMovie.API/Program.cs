using CleanMovie.API.Controllers;
using CleanMovie.API.Extensions;
using CleanMovie.Core.Entities;
using CleanMovie.Data;
using CleanMovie.Data.Repositories;
using CleanMovie.Service.Contracts;
using CleanMovie.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") // Allow your frontend
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
// Use built-in AddControllers since AddApiControllers extension is not available
builder.Services.AddControllers();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.MapControllers();

// I Program.cs (längst ner innan app.Run())
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MovieDbContext>();
    
    // Kör seeder-klassen
    DbSeeder.Seed(context);
}

app.Run();
