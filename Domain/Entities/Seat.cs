using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Seat
{
    public Guid Id { get; set; }
    public int SectorId { get; set; }
    public string RowIdentifier { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string Status { get; set; } = "Available"; // "Available", "Reserved", "Sold"

    // --- NUEVOS CAMPOS PARA EL BLOQUEO TEMPORAL ---
    public DateTime? LockUntil { get; set; } // Fecha y hora en que expira el bloqueo
    public int? LockedByUserId { get; set; } // ID del usuario que lo tiene "congelado"
    // ----------------------------------------------

    public Sector Sector { get; set; } = null!;

    [ConcurrencyCheck]
    public int Version { get; set; } 
}