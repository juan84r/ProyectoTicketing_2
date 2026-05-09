using Application.UseCases.Events.Queries;
using Application.Interfaces; // IMPORTANTE: Para IEventRepository
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/events")] 
public class EventsController : ControllerBase
{
    private readonly GetEventsHandler _getEventsHandler;
    private readonly GetSeatsBySectorHandler _getSeatsHandler;
    private readonly IEventRepository _eventRepository; // <--- 1. Declaramos la variable

    // 2. La agregamos al constructor para que .NET la inyecte
    public EventsController(
        GetEventsHandler getEventsHandler, 
        GetSeatsBySectorHandler getSeatsHandler,
        IEventRepository eventRepository) 
    {
        _getEventsHandler = getEventsHandler;
        _getSeatsHandler = getSeatsHandler;
        _eventRepository = eventRepository; // <--- 3. La asignamos
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getEventsHandler.HandleAsync();
        return Ok(result);
    }

    // NUEVO: Endpoint para obtener sectores de un evento específico
    [HttpGet("{eventId}/sectors")]
    public async Task<IActionResult> GetSectors(int eventId)
    {
        // Ahora _eventRepository ya existe en el contexto
        var eventData = await _eventRepository.GetEventByIdAsync(eventId);

        if (eventData == null) 
            return NotFound(new { message = "Evento no encontrado" });

        // Devolvemos los sectores mapeados a un objeto simple
        var response = eventData.Sectors.Select(s => new {
            id = s.Id,
            name = s.Name,
            capacity = s.Capacity,
            price = s.Price
        });

        return Ok(response);
    }

    [HttpGet("{sectorId}/seats")]
    public async Task<IActionResult> GetSeats(int sectorId)
    {
        var result = await _getSeatsHandler.HandleAsync(sectorId);

        if (result == null)
        {
            return NotFound(new { 
                message = $"No se encontraron asientos para el sector {sectorId}." 
            });
        }

        return Ok(result);
    }
}