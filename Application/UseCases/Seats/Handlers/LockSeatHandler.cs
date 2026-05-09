using Domain.Entities;
using Application.Interfaces;
using Application.UseCases.Seats.Commands;

namespace Application.UseCases.Seats.Handlers;

public class LockSeatHandler
{
    private readonly ISeatRepository _seatRepository;

    public LockSeatHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(LockSeatCommand command)
    {
        var seat = await _seatRepository.GetByIdAsync(command.SeatId);

        if (seat == null) return false;

        // Lógica: Está libre O el bloqueo ya venció
        bool isAvailable = seat.Status == "Available";
        bool isExpired = seat.Status == "Reserved" && seat.LockUntil < DateTime.UtcNow;

        if (isAvailable || isExpired)
        {
            seat.Status = "Reserved";
            seat.LockedByUserId = command.UserId;
            seat.LockUntil = DateTime.UtcNow.AddMinutes(5); // Bloqueo por 5 min
            
            seat.Version++; // Para evitar que dos personas ganen al mismo tiempo

            await _seatRepository.UpdateAsync(seat);
            await _seatRepository.SaveChangesAsync();
            return true;
        }

        return false; // El asiento está ocupado por otro y no venció
    }
}