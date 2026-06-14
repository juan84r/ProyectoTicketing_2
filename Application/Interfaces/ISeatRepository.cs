using Domain.Entities;

namespace Application.Interfaces;

public interface ISeatRepository
{
    // Mantenemos Guid para que CreateReservationHandler no falle
    Task<Seat?> GetByIdAsync(Guid id); 
    Task UpdateAsync(Seat seat);
    Task AddAsync(Seat seat); 
    Task SaveChangesAsync();
}