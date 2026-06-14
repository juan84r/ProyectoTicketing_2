using Application.Interfaces;
using Domain.Entities;
using Application.UseCases.Reservations;

public class CreateReservationHandler
{
    private readonly ISeatRepository _seatRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuditRepository _auditRepository;
    

    public CreateReservationHandler(
        ISeatRepository seatRepository,
        IReservationRepository reservationRepository,
        IAuditRepository auditRepository,
        IUserRepository userRepository)
    {
        _seatRepository = seatRepository;
        _reservationRepository = reservationRepository;
        _auditRepository = auditRepository;
        _userRepository = userRepository;
    }

    public async Task<ReservationResult> Handle(CreateReservationRequest request)
    {
        var seats = new List<Seat>();
        var user = await _userRepository.GetByIdAsync(request.UserId);

        // Buscamos al usuario al principio para tenerlo disponible en los logs
        if (user == null)
            return ReservationResult.UserNotFound;

        foreach (var seatId in request.SeatIds)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return ReservationResult.SeatNotFound;

            // --- CAMBIO CLAVE ACA ---
            bool isAvailable = seat.Status == "Available";
            bool isMyLock = seat.Status == "Reserved" && seat.LockedByUserId == request.UserId;

            if (!isAvailable && !isMyLock)
            {
                // NUEVO: Antes de tirar el error, dejamos asentado en la auditoria que asiento causo el conflicto
                var logFallo = new AuditLog
                {
                    UserId = user.Id,
                    Action = "Purchase Attempt Failed", // El nombre exacto de la accion fallida
                    EntityType = "Seat",
                    EntityId = seat.Id.ToString(),
                    Details = $"El usuario {user.Email} intentó comprar el asiento {seat.SeatNumber} pero ya estaba ocupado por otro usuario (Estado: {seat.Status}).",
                    Timestamp = DateTime.UtcNow
                };

                await _auditRepository.AddAsync(logFallo);
                await _auditRepository.SaveChangesAsync(); // Guardamos el log inmediatamente antes de salir

                return ReservationResult.SeatAlreadyReserved;
            }
            // -------------------------

            seats.Add(seat);
        }

        // Si paso el bucle anterior, significa que TODOS los asientos seleccionados estan aptos para la compra
        foreach (var seat in seats)
        {
            // Cambiamos a "Sold" (Vendido) para que ya no figure como reservado temporal
            seat.Status = "Sold"; 
            seat.LockedByUserId = null; // Limpiamos el bloqueo
            seat.LockUntil = null;      // Limpiamos el tiempo

            await _seatRepository.UpdateAsync(seat);

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                SeatId = seat.Id,
                UserId = request.UserId,
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5), 
                Status = "Completed",
                SeatNumber = seat.SeatNumber
            };

            await _reservationRepository.AddAsync(reservation);

            var log = new AuditLog
            {
                UserId = user.Id,
                Action = "Seat Purchased",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"Compra asiento {seat.SeatNumber}",
                Timestamp = DateTime.UtcNow
            };

            await _auditRepository.AddAsync(log);
        }

        await _seatRepository.SaveChangesAsync();

        return ReservationResult.Success;
    }
}