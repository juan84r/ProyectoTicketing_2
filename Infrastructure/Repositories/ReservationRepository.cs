using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

 public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
{
    return await _context.Reservations
        .Include(r => r.Seat)                           // Trae el Asiento
            .ThenInclude(s => s.Sector)                 // Trae el Sector
                .ThenInclude(sec => sec.Event)          // Trae el Evento
        .Where(r => r.UserId == userId)
        .OrderByDescending(r => r.ReservedAt)
        .ToListAsync();
}
}