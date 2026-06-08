using Domain.Entities;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event?> GetEventByIdAsync(int id);
    Task<Sector?> GetSectorByIdAsync(int sectorId);
    Task<int> GetTotalEventsAsync();
    Task AddAsync(Event newEvent);
    Task<int> SaveChangesAsync(); // Agregar esto
    
}