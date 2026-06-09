using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.Events.Queries;

public class GetSeatsBySectorHandler
{
    private readonly IEventRepository _eventRepository;

    public GetSeatsBySectorHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<SeatResponse>?> HandleAsync(int sectorId)
{
    var sector =
    await _eventRepository
        .GetSectorByIdAsync(sectorId);
    
    if (sector == null || sector.Seats == null) 
    {
        return null; 
    }

    // Mapeamos a SeatResponse
    return sector.Seats
        .OrderBy(s => s.SeatNumber) 
        .Select(s => new SeatResponse(
            s.Id, 
            s.RowIdentifier ?? "", 
            s.SeatNumber, 
            s.Status
        ));
}
}