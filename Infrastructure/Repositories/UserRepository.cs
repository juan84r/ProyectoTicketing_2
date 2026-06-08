using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task AddAsync(User user)
    {
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
    return await _context.Users.FindAsync(id);
    }
}
