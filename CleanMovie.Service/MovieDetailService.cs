using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Entities;
using CleanMovie.Service.Contracts;

namespace CleanMovie.Services;

public class MovieDetailService : IMovieDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    public MovieDetailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MovieDetails>> GetAllAsync()
    {
        return await _unitOfWork.MovieDetails.GetAllAsync();
    }

    public async Task<MovieDetails?> GetAsync(int id)
    {
        return await _unitOfWork.MovieDetails.GetAsync(id);
    }

    public async Task<MovieDetails> CreateAsync(MovieDetails details)
    {
        await _unitOfWork.MovieDetails.AddAsync(details);
        await _unitOfWork.CompleteAsync();

        return details;
    }

    public async Task UpdateAsync(MovieDetails details)
    {
        _unitOfWork.MovieDetails.Update(details);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var details = await _unitOfWork.MovieDetails.GetAsync(id);

        if (details is null)
            return;

        _unitOfWork.MovieDetails.Delete(details);
        await _unitOfWork.CompleteAsync();
    }
}
