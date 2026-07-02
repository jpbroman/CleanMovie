using CleanMovie.Core.DomainContracts;
using CleanMovie.Service.Contracts;
using CleanMovie.Core.Entities;

namespace CleanMovie.Data.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _unitOfWork.Reviews.GetAllAsync();
    }

    public async Task<IEnumerable<Review>> GetByMovieAsync(int movieId)
    {
        return await _unitOfWork.Reviews.GetByMovieIdAsync(movieId);
    }

    public async Task<Review?> GetAsync(int id)
    {
        return await _unitOfWork.Reviews.GetAsync(id);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.CompleteAsync();

        return review;
    }

    public async Task UpdateAsync(Review review)
    {
        _unitOfWork.Reviews.Update(review);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var review = await _unitOfWork.Reviews.GetAsync(id);

        if (review is null)
            return;

        _unitOfWork.Reviews.Delete(review);
        await _unitOfWork.CompleteAsync();
    }
}
