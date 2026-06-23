using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Models;
using CleanMovie.Core.Models.DTOs;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _repository;
    private readonly IMovieDbContext _context;

    public MoviesController(
        IMovieRepository repository,
        IMovieDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll()
    {
        var movies = await _repository.GetAllAsync();

        var result = movies.Select(movie => new MovieDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Genre,
            movie.Duration,
            movie.Details is null
                ? null
                : new MovieDetailsDto(
                    movie.Details.Synopsis,
                    movie.Details.Language,
                    movie.Details.Budget),
            movie.MovieActors.Select(ma => new ActorDto(
                ma.Actor.Id,
                ma.Actor.Name,
                ma.Actor.BirthDate)),
            movie.Reviews.Select(r => new ReviewDto(
                r.Id,
                r.Reviewer,
                r.Comment,
                r.Rating))
        ));

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDto>> Get(int id)
    {
        var movie = await _repository.GetAsync(id);

        if (movie is null)
            return NotFound();

        var dto = new MovieDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Genre,
            movie.Duration,
            movie.Details is null
                ? null
                : new MovieDetailsDto(
                    movie.Details.Synopsis,
                    movie.Details.Language,
                    movie.Details.Budget),
            movie.MovieActors.Select(ma => new ActorDto(
                ma.Actor.Id,
                ma.Actor.Name,
                ma.Actor.BirthDate)),
            movie.Reviews.Select(r => new ReviewDto(
                r.Id,
                r.Reviewer,
                r.Comment,
                r.Rating)));

        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<ActionResult<Movie>> Create(Movie movie)
    {
        await _repository.AddAsync(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest();

        var existing = await _repository.GetAsync(id);

        if (existing is null)
            return NotFound();

        _repository.Update(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _repository.GetAsync(id);

        if (movie is null)
            return NotFound();

        _repository.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
