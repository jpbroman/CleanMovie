using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Models;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieDetailsController : ControllerBase
{
    private readonly IMovieDetailsRepository _repository;
    private readonly IMovieDbContext _context;

    public MovieDetailsController(
        IMovieDetailsRepository repository,
        IMovieDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDetails>>> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetails>> Get(int id)
    {
        var details = await _repository.GetAsync(id);

        if (details is null)
            return NotFound();

        return Ok(details);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDetails>> Create(MovieDetails details)
    {
        await _repository.AddAsync(details);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = details.Id }, details);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var details = await _repository.GetAsync(id);

        if (details is null)
            return NotFound();

        _repository.Delete(details);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

