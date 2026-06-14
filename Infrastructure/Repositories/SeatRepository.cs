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

    public Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        return Task.CompletedTask;
    }

    public async Task AddAsync(Seat seat)
    {
        // Preparamos el asiento para ser insertado en la tabla Seats
        await _context.Seats.AddAsync(seat);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}