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
    private readonly UnlockSeatHandler _unlockHandler;

    public SeatsController(LockSeatHandler lockHandler, ISeatRepository seatRepository, UnlockSeatHandler unlockHandler)
    {
        _lockHandler = lockHandler;
        _seatRepository = seatRepository;
        _unlockHandler = unlockHandler;
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
    public async Task<IActionResult> UnlockSeat(
        [FromBody] LockSeatCommand command)
    {
        var result =
            await _unlockHandler.Handle(
                command.SeatId,
                command.UserId);

        if (result)
        {
            return Ok(new
            {
                message = "Asiento liberado correctamente"
            });
        }

        return BadRequest(new
        {   
            message = "No se pudo liberar el asiento"
        });
    }
}