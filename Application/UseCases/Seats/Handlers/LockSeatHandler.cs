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
        // Buscar usuario para la auditoria (afuera del bucle para no sobrecargar consultas)
        var user = await _userRepository.GetByIdAsync(command.UserId);
        
        // Creamos una lista temporal en memoria para acumular los asientos aptos
        var seatsToLock = new List<Seat>();

        // ==========================================================================
        // FASE 1: VALIDACION ATOMICA (Verificar si todo el paquete esta libre)
        // ==========================================================================
        foreach (var seatId in command.SeatIds)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return false; // Si un ID no existe en la DB, cancelamos todo el proceso

            bool isAvailable = seat.Status == "Available";
            bool isExpired = seat.Status == "Reserved" && seat.LockUntil < DateTime.UtcNow;

            // Si el asiento NO esta disponible Y tampoco vencio su reserva previa...
            if (!isAvailable && !isExpired)
            {
                // Registramos en auditoria que este intento masivo fallo debido a este asiento ocupado
                await _auditRepository.AddAsync(new AuditLog
                {
                    UserId = user?.Id,
                    Action = "Reservation Failed - Seat Occupied",
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = $"Conflicto: Intento de bloqueo masivo falló por asiento {seat.SeatNumber} ocupado.",
                    Timestamp = DateTime.UtcNow
                });

                await _seatRepository.SaveChangesAsync();

                return false; // Cortamos la ejecucion. No se bloquea NADA del grupo.
            }

            // Si esta apto, lo guardamos temporalmente en nuestra lista de memoria
            seatsToLock.Add(seat);
        }

        // ==========================================================================
        // FASE 2: APLICACION DEL BLOQUEO (Solo llegamos aca si TODOS estaban libres)
        // ==========================================================================
        foreach (var seat in seatsToLock)
        {
            seat.Status = "Reserved";
            seat.LockedByUserId = command.UserId;
            seat.LockUntil = DateTime.UtcNow.AddMinutes(5);

            // Incrementamos la version para el control de concurrencia optimista
            seat.Version++;

            // Marcamos el asiento modificado en el rastreador de Entity Framework
            await _seatRepository.UpdateAsync(seat);

            // Grabamos una auditoria individual por cada asiento congelado con exito
            await _auditRepository.AddAsync(new AuditLog
            {
                UserId = user?.Id,
                Action = "Seat Temporarily Reserved",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"Bloqueo temporal masivo - Asiento {seat.SeatNumber}",
                Timestamp = DateTime.UtcNow
            });
        }

        // Impactamos todos los cambios juntos en la base de datos en una unica transaccion
        await _seatRepository.SaveChangesAsync();

        return true;
    }
}