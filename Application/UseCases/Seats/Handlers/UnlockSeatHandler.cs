using Domain.Entities;
using Application.Interfaces;
using Application.UseCases.Seats.Commands;
using System;
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
        // 1. VALIDACION: Si no mandaron IDs, cortamos de una
        if (command.SeatIds == null || !command.SeatIds.Any())
            return false;

        // Buscamos el usuario una sola vez afuera del bucle
        var user = await _userRepository.GetByIdAsync(command.UserId);
        bool alMenosUnoLiberado = false;

        // 2. SOLUCION: Recorremos TODO el array de IDs enviados por React
        foreach (var seatId in command.SeatIds)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            // Validamos que el asiento exista y que realmente le pertenezca al usuario que lo quiere liberar
            if (seat != null && seat.LockedByUserId == command.UserId)
            {
                seat.Status = "Available";
                seat.LockedByUserId = null;
                seat.LockUntil = null;
                seat.Version++; // Mantenemos el control de concurrencia

                await _seatRepository.UpdateAsync(seat);
                
                // Registramos la auditoria individual para este asiento liberado
                await _auditRepository.AddAsync(new AuditLog
                {
                    UserId = user?.Id,
                    Action = "Seat Unlocked",
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = $"El usuario liberó el asiento {seat.SeatNumber} manualmente.",
                    Timestamp = DateTime.UtcNow
                });

                alMenosUnoLiberado = true;
            }
        }

        // 3. IMPACTO EN BASE DE DATOS: Si se modifico al menos un asiento, guardamos los cambios juntos
        if (alMenosUnoLiberado)
        {
            await _seatRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }
}