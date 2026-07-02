using Microsoft.AspNetCore.Mvc;
using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IServiceManager _services;

    public ActorsController(IServiceManager services)
    {
        _services = services;
    }

    /// <summary>
    /// Get all actors.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> GetAll()
    {
        return Ok(await _services.Actors.GetAllAsync());
    }

    /// <summary>
    /// Get specific actor.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Actor>> Get(int id)
    {
        var actor = await _services.Actors.GetAsync(id);

        if (actor is null)
            return NotFound();

        return Ok(actor);
    }

    /// <summary>
    /// Add a new actor.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Actor>> Create(Actor actor)
    {
        var created = await _services.Actors.CreateAsync(actor);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update info on existing actor.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Actor actor)
    {
        if (id != actor.Id)
            return BadRequest();

        await _services.Actors.UpdateAsync(actor);

        return NoContent();
    }

    /// <summary>
    /// Delete an actor.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _services.Actors.DeleteAsync(id);

        return NoContent();
    }
}
