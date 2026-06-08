using Domain.Entities;

namespace Application.Interfaces;

public interface IReservationRepository
{
    Task AddAsync(Reservation reservation);
    
    // NUEVO: Metodo para obtener las reservas de un usuario especifico
    Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId);
}