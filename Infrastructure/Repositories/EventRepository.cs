using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
{
    // Agregamos los Includes para que la lista no venga "pelada"
    return await _context.Events
        .Include(e => e.Sectors)         // Trae los sectores de cada evento
            .ThenInclude(s => s.Seats)   // Trae los asientos de cada sector
        .ToListAsync();
}

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Sectors)
                .ThenInclude(s => s.Seats)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Event newEvent)
    {
        await _context.Events.AddAsync(newEvent);
        // Quitamos el SaveChanges de acá para que el Handler tenga el control total
    }

    // --- AGREGÁ ESTE MÉTODO PARA SOLUCIONAR EL ERROR ---
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}