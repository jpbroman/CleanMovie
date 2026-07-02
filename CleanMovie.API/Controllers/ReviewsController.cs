using Microsoft.AspNetCore.Mvc;
using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IServiceManager _services;

    public ReviewsController(IServiceManager services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAll()
    {
        return Ok(await _services.Reviews.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Review>> Get(int id)
    {
        var review = await _services.Reviews.GetAsync(id);

        if (review is null)
            return NotFound();

        return Ok(review);
    }

    [HttpGet("movie/{movieId:int}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetByMovie(int movieId)
    {
        return Ok(await _services.Reviews.GetByMovieAsync(movieId));
    }

    [HttpPost]
    public async Task<ActionResult<Review>> Create(Review review)
    {
        var created = await _services.Reviews.CreateAsync(review);

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Review review)
    {
        if (id != review.Id)
            return BadRequest();

        await _services.Reviews.UpdateAsync(review);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _services.Reviews.DeleteAsync(id);

        return NoContent();
    }
}

