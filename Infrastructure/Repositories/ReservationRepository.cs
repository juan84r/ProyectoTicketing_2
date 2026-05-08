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

    // AGREGÁ ESTE MÉTODO:
  public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
{
    return await _context.Reservations
        .Where(r => r.UserId == userId)
        .ToListAsync();
}
}