using CleanMovie.Core.Entities;
namespace CleanMovie.Service.Contracts;

public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<Actor?> GetAsync(int id);
    Task<Actor> CreateAsync(Actor actor);
    Task UpdateAsync(Actor actor);
    Task DeleteAsync(int id);
}
