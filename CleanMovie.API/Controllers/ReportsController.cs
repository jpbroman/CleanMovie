using Microsoft.AspNetCore.Mvc;
using CleanMovie.Core.DomainContracts;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMovieService _movieService;

    // Injicera MovieService istället för UnitOfWork
    public ReportsController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReport()
    {
        var stats = await _movieService.GetDashboardStatisticsAsync();
        return Ok(stats);
    }
}
