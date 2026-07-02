using Microsoft.AspNetCore.Mvc;
using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieDetailsController : ControllerBase
{
    private readonly IServiceManager _services;

    public MovieDetailsController(IServiceManager services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDetails>>> GetAll()
    {
        return Ok(await _services.MovieDetails.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetails>> Get(int id)
    {
        var details = await _services.MovieDetails.GetAsync(id);

        if (details is null)
            return NotFound();

        return Ok(details);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDetails>> Create(MovieDetails details)
    {
        var created = await _services.MovieDetails.CreateAsync(details);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MovieDetails details)
    {
        if (id != details.Id)
            return BadRequest();

        await _services.MovieDetails.UpdateAsync(details);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _services.MovieDetails.DeleteAsync(id);

        return NoContent();
    }
}
