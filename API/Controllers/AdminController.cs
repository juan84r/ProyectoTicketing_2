using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Events.Handlers;
using Application.UseCases.Events.Commands;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
// Base limpia y unica para el administrador
[Route("api/v1/admin/events")] 
public class AdminController : ControllerBase
{
    private readonly GenerateEventHandler _handler;

    public AdminController(GenerateEventHandler handler)
    {
        _handler = handler;
    }

    // Al poner el HttpPost vacio, hereda la ruta de la clase: "api/v1/admin/events"
    [HttpPost] 
    public async Task<IActionResult> Generate([FromBody] GenerateEventCommand command)
    {
        var result = await _handler.Handle(command);
    
        if (result) 
        {
            return StatusCode(201, new { message = "¡Evento, sectores y asientos generados con éxito!" });
        }

        return BadRequest("Hubo un error al procesar la generación masiva.");
    }
}