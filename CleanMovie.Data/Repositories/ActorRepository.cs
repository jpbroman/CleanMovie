using CleanMovie.Core.Interfaces;
using CleanMovie.Core.Models;
using Microsoft.EntityFrameworkCore;

public class ActorRepository : IActorRepository
{
    private readonly IMovieDbContext _context;

    public ActorRepository(IMovieDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        return await _context.Actors.ToListAsync();
    }

    public async Task<Actor?> GetAsync(int id)
    {
        return await _context.Actors.FindAsync(id);
    }

    public async Task AddAsync(Actor actor)
    {
        await _context.Actors.AddAsync(actor);
    }

    public void Update(Actor actor)
    {
        _context.Actors.Update(actor);
    }

    public void Delete(Actor actor)
    {
        _context.Actors.Remove(actor);
    }
}
