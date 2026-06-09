using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Seats.Handlers;
using Application.UseCases.Seats.Commands;
using Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly LockSeatHandler _lockHandler;
    private readonly UnlockSeatHandler _unlockHandler; // 1. Agregamos la variable del nuevo handler
    private readonly ISeatRepository _seatRepository;

    // 2. Lo inyectamos en el constructor
    public SeatsController(
        LockSeatHandler lockHandler, 
        UnlockSeatHandler unlockHandler, 
        ISeatRepository seatRepository)
    {
        _lockHandler = lockHandler;
        _unlockHandler = unlockHandler;
        _seatRepository = seatRepository;
    }

    [HttpPost("lock")]
    public async Task<IActionResult> LockSeat([FromBody] LockSeatCommand command)
    {
        var result = await _lockHandler.Handle(command);
        if (result)
            return Ok(new { message = "Asientos bloqueados por 5 minutos" });
        
        return Conflict(new { message = "Uno o más asientos ya no están disponibles" });
    }

    [HttpPost("unlock")]
    public async Task<IActionResult> UnlockSeat([FromBody] LockSeatCommand command)
    {
        // 3. Dejamos que el Handler haga su trabajo
        var result = await _unlockHandler.Handle(command);
        
        if (result)
            return Ok(new { message = "Asiento liberado correctamente" });

        return BadRequest(new { message = "No se pudo liberar el asiento" });
    }
}