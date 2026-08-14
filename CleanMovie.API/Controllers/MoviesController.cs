using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Entities.DTOs;
using CleanMovie.Core.Entities;
using CleanMovie.Service.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

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
    /// [AllowAnonymous]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll([FromQuery] QueryParameters qp)
    {
        _logger.LogInformation("Get all movies");
        return Ok(await _services.Movies.GetAllAsync(qp));
    }

    /// <summary>
    /// Get unique movie detailed.
    /// </summary>
    [AllowAnonymous]
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
//    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create(CreateMovieDto dto)
    {
        var movie = await _services.Movies.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }

    /// <summary>
    /// Update an existing movie..
    /// </summary>
    [AllowAnonymous]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateMovieDto dto)
    {
        _logger.LogInformation($"Updating movie {dto.Title}");

        await _services.Movies.UpdateAsync(id, dto);

        return NoContent();
    }
    
    /// <summary>
    /// Remove specified movie.
    /// </summary>
    [AllowAnonymous]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Deleting movie with Id {id}");
        await _services.Movies.DeleteAsync(id);

        return NoContent();
    }
}

