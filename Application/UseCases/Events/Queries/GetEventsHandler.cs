using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Events.Queries;

public class GetEventsHandler
{
    private readonly IEventRepository _eventRepository;

    public GetEventsHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<PagedEventsResponse> HandleAsync(int page, int pageSize)
{
    // 1. Traemos la porcion exacta ya paginada desde PostgreSQL
    var events = await _eventRepository.GetAllEventsAsync(page, pageSize);

    // 2. Traemos el total de filas para calcular los botones en React
    var total = await _eventRepository.GetTotalEventsAsync();
 
    // Solo mapeamos lo que vino de la DB al DTO de salida.
    var pagedEvents = events
        .Select(e => new EventResponse(
            e.Id,
            e.Name,
            e.EventDate,
            e.Venue,
            e.Status
        ))
        .ToList();

    return new PagedEventsResponse
    {
        Total = total,
        HasNext = page * pageSize < total,
        Data = pagedEvents
    };
}
}