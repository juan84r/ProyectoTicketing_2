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

    public async Task<PagedEventsResponse> HandleAsync(
        int page,
        int pageSize)
    {
        var events = await _eventRepository.GetAllEventsAsync();

        var total = await _eventRepository.GetTotalEventsAsync();

        var pagedEvents = events
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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