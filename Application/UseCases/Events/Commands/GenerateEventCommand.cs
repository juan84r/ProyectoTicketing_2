namespace Application.UseCases.Events.Commands;

public class GenerateEventCommand
{
    public string Name { get; set; } = string.Empty;
    public int NumSectors { get; set; }
    public int RowsPerSector { get; set; }
    public int SeatsPerRow { get; set; }
    public decimal Price { get; set; }
}