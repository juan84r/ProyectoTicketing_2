namespace Application.DTOs;

public record EventResponse(
    int Id, 
    string Name, 
    DateTime EventDate, 
    string Venue, 
    string Status
);