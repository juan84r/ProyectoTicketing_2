using System;
using System.Collections.Generic;

namespace Application.UseCases.Seats.Commands;

public class LockSeatCommand
{
    // Cambiado a lista para que acepte múltiples asientos de React
    public List<Guid> SeatIds { get; set; } = new();
    public int UserId { get; set; }
}