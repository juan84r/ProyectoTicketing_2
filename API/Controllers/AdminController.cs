using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Events.Handlers;
using Application.UseCases.Events.Commands;

namespace API.Controllers;

[ApiController]
[Route("api/v1/admin")]
public class AdminController : ControllerBase
{
    private readonly GenerateEventHandler _handler;

    // El controlador SOLO necesita el Handler, nada de DbContext
    public AdminController(GenerateEventHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("events/generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateEventCommand command)
    {
        var result = await _handler.Handle(command);
        
        if (result) 
        {
            return Ok(new { message = "¡Evento, sectores y asientos generados con éxito!" });
        }

        return BadRequest("Hubo un error al procesar la generación masiva.");
    }
}