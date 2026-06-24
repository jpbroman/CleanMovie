using CleanMovie.Core.Entities;

namespace CleanMovie.Core.Interfaces;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<Actor?> GetAsync(int id);
    Task AddAsync(Actor actor);
    void Update(Actor actor);
    void Delete(Actor actor);
}
