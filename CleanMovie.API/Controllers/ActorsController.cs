using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IActorRepository _repository;
    private readonly IMovieDbContext _context;

    public ActorsController(
        IActorRepository repository,
        IMovieDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Actor>> Get(int id)
    {
        var actor = await _repository.GetAsync(id);

        if (actor is null)
            return NotFound();

        return Ok(actor);
    }

    [HttpPost]
    public async Task<ActionResult<Actor>> Create(Actor actor)
    {
        await _repository.AddAsync(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = actor.Id }, actor);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Actor actor)
    {
        if (id != actor.Id)
            return BadRequest();

        var existing = await _repository.GetAsync(id);

        if (existing is null)
            return NotFound();

        _repository.Update(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var actor = await _repository.GetAsync(id);

        if (actor is null)
            return NotFound();

        _repository.Delete(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
