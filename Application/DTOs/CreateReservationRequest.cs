namespace Application.UseCases.Reservations;


public class CreateReservationRequest
{
    public int UserId { get; set; }
    public List<Guid> SeatIds { get; set; } = new();
}