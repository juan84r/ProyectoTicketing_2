using Domain.Entities;
using Application.Interfaces;
using Application.UseCases.Seats.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.Seats.Handlers;

public class UnlockSeatHandler
{
    private readonly ISeatRepository _seatRepository;
    private readonly IAuditRepository _auditRepository;
    private readonly IUserRepository _userRepository;

    public UnlockSeatHandler(
        ISeatRepository seatRepository,
        IAuditRepository auditRepository,
        IUserRepository userRepository)
    {
        _seatRepository = seatRepository;
        _auditRepository = auditRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(LockSeatCommand command)
    {
        if (command.SeatIds == null || !command.SeatIds.Any())
            return false;

        var user = await _userRepository.GetByIdAsync(command.UserId);
        var seat = await _seatRepository.GetByIdAsync(command.SeatIds[0]);

        if (seat != null && seat.LockedByUserId == command.UserId)
        {
            seat.Status = "Available";
            seat.LockedByUserId = null;
            seat.LockUntil = null;
            seat.Version++;

            await _seatRepository.UpdateAsync(seat);
            
            // Agregamos auditoria para que quede registro de que el usuario cancelo
            await _auditRepository.AddAsync(new AuditLog
            {
                UserId = user?.Id,
                Action = "Seat Unlocked",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"El usuario liberó el asiento {seat.SeatNumber} manualmente",
                Timestamp = DateTime.UtcNow
            });

            await _seatRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }
}