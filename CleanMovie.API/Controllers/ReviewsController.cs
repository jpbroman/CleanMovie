using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Entities;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewRepository _repository;
    private readonly IMovieDbContext _context;

    public ReviewsController(
        IReviewRepository repository,
        IMovieDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Review>> Get(int id)
    {
        var review = await _repository.GetAsync(id);

        if (review is null)
            return NotFound();

        return Ok(review);
    }

    [HttpGet("movie/{movieId:int}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetByMovie(int movieId)
    {
        return Ok(await _repository.GetByMovieIdAsync(movieId));
    }

    [HttpPost]
    public async Task<ActionResult<Review>> Create(Review review)
    {
        await _repository.AddAsync(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = review.Id }, review);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _repository.GetAsync(id);

        if (review is null)
            return NotFound();

        _repository.Delete(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

