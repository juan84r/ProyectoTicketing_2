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

    public async Task<IEnumerable<Event>> GetAllEventsAsync(int page, int pageSize)
{
    // La base de datos ahora filtra, ordena y corta la porcion exacta
    return await _context.Events
        .AsNoTracking()                  // 1. Apaga el rastreo (ahorra muchisima RAM)
        .Include(e => e.Sectors)         // 2. Trae los sectores si los necesita
        .OrderBy(e => e.Id)              // 3. Ordenamos por ID (obligatorio para el Skip)
        .Skip((page - 1) * pageSize)     // 4. Salta los eventos de paginas anteriores
        .Take(pageSize)                  // 5. Agarra solo la cantidad solicitada
        .ToListAsync();                  // 6. Recien aca viaja a PostgreSQL
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
        // Quitamos el SaveChanges de aca para que el Handler tenga el control total
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<Sector?> GetSectorByIdAsync(int sectorId)
    {
        return await _context.Sectors
            .Include(s => s.Seats) // Incluimos los asientos del sector
            .FirstOrDefaultAsync(s => s.Id == sectorId);
    }
    
    public async Task<int> GetTotalEventsAsync()
    {
    return await _context.Events.CountAsync();
    }
}