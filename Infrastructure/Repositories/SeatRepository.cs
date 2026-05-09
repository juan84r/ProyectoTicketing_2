using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly AppDbContext _context;

    public SeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Seat?> GetByIdAsync(Guid id)
    {
        return await _context.Seats.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        // Nota: El SaveChanges se suele llamar al final de la transacción en el Handler
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Seat seat)
    {
        // Preparamos el asiento para ser insertado en la tabla Seats
        await _context.Seats.AddAsync(seat);
    }
}