using Domain.Entities;
using Application.Interfaces;
using Application.UseCases.Seats.Commands;

namespace Application.UseCases.Seats.Handlers;

public class LockSeatHandler
{
    private readonly ISeatRepository _seatRepository;

    private readonly IAuditRepository _auditRepository;

    private readonly IUserRepository _userRepository;

    public LockSeatHandler(
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
        var seat = await _seatRepository.GetByIdAsync(command.SeatId);

        if (seat == null)
            return false;

        // Buscar usuario para auditoría
        var user = await _userRepository.GetByIdAsync(command.UserId);

        // Lógica:
        // Disponible
        // O bloqueo vencido
        bool isAvailable =
            seat.Status == "Available";

        bool isExpired =
            seat.Status == "Reserved" &&
            seat.LockUntil < DateTime.UtcNow;

        // =========================
        // BLOQUEO EXITOSO
        // =========================
        if (isAvailable || isExpired)
        {
            seat.Status = "Reserved";

            seat.LockedByUserId =
                command.UserId;

            seat.LockUntil =
                DateTime.UtcNow.AddMinutes(5);

            // Control concurrencia
            seat.Version++;

            await _seatRepository.UpdateAsync(seat);

            await _seatRepository.SaveChangesAsync();

            // AUDITORÍA
            await _auditRepository.AddAsync(
                new AuditLog
                {
                    UserId = user?.Id,
                    Action = "Seat Temporarily Reserved",
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = $"Bloqueo temporal asiento {seat.SeatNumber}",
                    Timestamp = DateTime.UtcNow
                });

            return true;
        }

        // =========================
        // FALLÓ POR CONCURRENCIA
        // =========================
        await _auditRepository.AddAsync(
            new AuditLog
            {
                UserId = user?.Id,
                Action = "Reservation Failed - Concurrency",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"Conflicto concurrencia asiento {seat.SeatNumber}",
                Timestamp = DateTime.UtcNow
            });

        return false;
    }
}