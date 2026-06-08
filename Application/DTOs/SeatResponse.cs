namespace Application.DTOs;

public record SeatResponse(
    Guid Id,
    string RowIdentifier,
    int SeatNumber,
    string Status
);