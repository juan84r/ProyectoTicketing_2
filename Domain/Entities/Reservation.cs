namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }

    public Guid SeatId { get; set; }

    public int UserId { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string Status { get; set; } = "Reserved";

    public int SeatNumber { get; set; }

    public Seat Seat { get; set; } = null!;
}