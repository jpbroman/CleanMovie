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

    public MoviesController(IServiceManager services)
    {
        _services = services;
    }

    /// <summary>
    /// Get all movies detailed.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll()
    {
        return Ok(await _services.Movies.GetAllAsync());
    }

    /// <summary>
    /// Get unique movie detailed.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDto>> Get(int id)
    {
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
        var created = await _services.Movies.CreateAsync(movie);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update an existing movie..
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Movie movie)
    {
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
        await _services.Movies.DeleteAsync(id);

        return NoContent();
    }
}

