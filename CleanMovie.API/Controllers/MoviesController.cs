using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Core.Entities;
using CleanMovie.Service.Contracts;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IServiceManager _services;
    private readonly ILogger<MoviesController> _logger;
    
    public MoviesController(IServiceManager services, ILogger<MoviesController> logger)
    {
        _logger = logger;
        _services = services;
    }

    /// <summary>
    /// Get all movies detailed.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll([FromQuery] QueryParameters qp)
    {
        _logger.LogInformation("Get all movies");
        return Ok(await _services.Movies.GetAllAsync(qp));
    }

    /// <summary>
    /// Get unique movie detailed.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDto>> Get(int id)
    {
        _logger.LogInformation($"Get movie with Id {id}");
        var movie = await _services.Movies.GetAsync(id);

        if (movie is null)
            return NotFound();

        return Ok(movie);
    }

    /// <summary>
    /// Add a new movie.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Movie>> Create(Movie movie)
    {
        _logger.LogInformation($"Adding movie {movie.Title}");
        var created = await _services.Movies.CreateAsync(movie);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update an existing movie..
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Movie movie)
    {
        _logger.LogInformation($"Updating movie {movie.Title}");
        if (id != movie.Id)
            return BadRequest();

        await _services.Movies.UpdateAsync(movie);

        return NoContent();
    }

    /// <summary>
    /// Remove specified movie.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Deleting movie with Id {id}");
        await _services.Movies.DeleteAsync(id);

        return NoContent();
    }
}

