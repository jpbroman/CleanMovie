using CleanMovie.Service.Contracts;

using CleanMovie.Core.DomainContracts;
using CleanMovie.Core.Entities;

namespace CleanMovie.Services;

public class ActorService : IActorService
{
    private readonly IUnitOfWork _unitOfWork;

    public ActorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        return await _unitOfWork.Actors.GetAllAsync();
    }

    public async Task<Actor?> GetAsync(int id)
    {
        return await _unitOfWork.Actors.GetAsync(id);
    }

    public async Task<Actor> CreateAsync(Actor actor)
    {
        await _unitOfWork.Actors.AddAsync(actor);
        await _unitOfWork.CompleteAsync();

        return actor;
    }

    public async Task UpdateAsync(Actor actor)
    {
        _unitOfWork.Actors.Update(actor);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var actor = await _unitOfWork.Actors.GetAsync(id);

        if (actor is null)
            return;

        _unitOfWork.Actors.Delete(actor);
        await _unitOfWork.CompleteAsync();
    }
}
