using Domain.Entities;
using Application.Interfaces;
using Application.UseCases.Events.Commands;

namespace Application.UseCases.Events.Handlers;

public class GenerateEventHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly ISeatRepository _seatRepository;

    public GenerateEventHandler(IEventRepository eventRepository, ISeatRepository seatRepository)
    {
        _eventRepository = eventRepository;
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(GenerateEventCommand command)
{
    try
    {
        // 1. Crear Evento - MODIFICADO
        var newEvent = new Event 
        { 
            Name = command.Name,
            Venue = command.Venue 
        };
        
        await _eventRepository.AddAsync(newEvent);
        await _eventRepository.SaveChangesAsync();

        // 2. Generar Sectores
        for (int i = 0; i < command.NumSectors; i++)
        {
            var sector = new Sector {
                Name = $"Sector {((char)('A' + i))}",
                EventId = newEvent.Id,
                Price = command.Price 
            };
            
            if (newEvent.Sectors == null) newEvent.Sectors = new List<Sector>();
            newEvent.Sectors.Add(sector);
            await _eventRepository.SaveChangesAsync(); 

            // 3. Generar Asientos
            int contador = 1;
            for (int r = 1; r <= command.RowsPerSector; r++)
            {
                for (int s = 1; s <= command.SeatsPerRow; s++)
                {
                    var seat = new Seat {
                        SectorId = sector.Id,
                        SeatNumber = contador,
                        Status = "Available"
                    };
                    await _seatRepository.AddAsync(seat);
                    contador++;
                }
            }
        }
        await _eventRepository.SaveChangesAsync();
        return true;
    }
    catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
        return false;
    }
}
}