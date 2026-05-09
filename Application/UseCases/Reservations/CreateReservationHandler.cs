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
    // Validamos primero todos los datos para evitar modificaciones parciales en la base de datos
    var seats = new List<Seat>();

    foreach (var seatId in request.SeatIds)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return ReservationResult.SeatNotFound;

        if (seat.Status != "Available")
            return ReservationResult.SeatAlreadyReserved;

        seats.Add(seat);
    }

    var user = await _userRepository.GetByIdAsync(request.UserId);

    if (user == null)
        return ReservationResult.UserNotFound;

    
    foreach (var seat in seats)
    {
        seat.Status = "Reserved";
        await _seatRepository.UpdateAsync(seat);

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            SeatId = seat.Id,
            UserId = request.UserId,
            ReservedAt = DateTime.UtcNow,
            SeatNumber = seat.SeatNumber
        };

        await _reservationRepository.AddAsync(reservation);

        var log = new AuditLog
        {
            Action = "Seat Reserved",
            User = user.Email, // Se usa email en auditoría para que el registro sea legible
            Resource = $"Asiento {seat.SeatNumber}",
            Timestamp = DateTime.UtcNow
        };

        await _auditRepository.AddAsync(log);
    }

    return ReservationResult.Success;
}
}