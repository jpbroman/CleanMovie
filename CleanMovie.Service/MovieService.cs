using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;
using CleanMovie.Core.DomainContracts;

namespace CleanMovie.Service;


public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;

    public MovieService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _unitOfWork.Movies.GetAllAsync();
    }

    public async Task<Movie?> GetAsync(int id)
    {
        return await _unitOfWork.Movies.GetAsync(id);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return movie;
    }

    public async Task UpdateAsync(Movie movie)
    {
        _unitOfWork.Movies.Update(movie);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetAsync(id);

        if (movie is null)
            return;

        _unitOfWork.Movies.Remove(movie);

        await _unitOfWork.CompleteAsync();
    }
}