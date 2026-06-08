using Domain.Entities;


namespace Application.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(AuditLog log);
    Task<IEnumerable<AuditLog>> GetAllAsync();
    
}

