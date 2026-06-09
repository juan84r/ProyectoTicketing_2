using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Seats.Handlers;
using Application.UseCases.Seats.Commands;
using Application.Interfaces; // Para el repositorio

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly LockSeatHandler _lockHandler;
    private readonly ISeatRepository _seatRepository; // Agregamos esto

    public SeatsController(LockSeatHandler lockHandler, ISeatRepository seatRepository)
    {
        _lockHandler = lockHandler;
        _seatRepository = seatRepository;
    }

    [HttpPost("lock")]
    public async Task<IActionResult> LockSeat([FromBody] LockSeatCommand command)
    {
        var result = await _lockHandler.Handle(command);
        if (result)
            return Ok(new { message = "Asiento bloqueado por 5 minutos" });
        
        return Conflict(new { message = "El asiento ya no está disponible" });
    }

    [HttpPost("unlock")]
    public async Task<IActionResult> UnlockSeat([FromBody] LockSeatCommand command)
    {
        // Usamos el repositorio directamente para buscar el asiento
        var seat = await _seatRepository.GetByIdAsync(command.SeatId);

        // Validamos que el asiento exista y que quien lo quiera liberar sea quien lo bloqueó
        if (seat != null && seat.LockedByUserId == command.UserId)
        {
            seat.Status = "Available";
            seat.LockedByUserId = null;
            seat.LockUntil = null;
            seat.Version++; // Mantenemos la consistencia de la versión

            await _seatRepository.UpdateAsync(seat);
            await _seatRepository.SaveChangesAsync();

            return Ok(new { message = "Asiento liberado correctamente" });
        }

        return BadRequest(new { message = "No se pudo liberar el asiento" });
    }
}