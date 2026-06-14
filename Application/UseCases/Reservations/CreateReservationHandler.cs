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

        foreach (var seatId in request.SeatIds)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return ReservationResult.SeatNotFound;

            // --- CAMBIO CLAVE ACA ---
            // Un asiento se puede comprar si:
            // 1. Esta "Available"
            // 2. Esta "Reserved" PERO el LockedByUserId es el mismo que el de la request
            bool isAvailable = seat.Status == "Available";
            bool isMyLock = seat.Status == "Reserved" && seat.LockedByUserId == request.UserId;

            if (!isAvailable && !isMyLock)
                return ReservationResult.SeatAlreadyReserved;
            // -------------------------

            seats.Add(seat);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
            return ReservationResult.UserNotFound;

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