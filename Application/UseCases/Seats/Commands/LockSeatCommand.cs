namespace Application.UseCases.Seats.Commands;

public class LockSeatCommand
{
    public Guid SeatId { get; set; }
    public int UserId { get; set; }
}