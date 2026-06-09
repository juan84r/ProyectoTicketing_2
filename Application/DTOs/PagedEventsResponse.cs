namespace Application.DTOs;

public class PagedEventsResponse
{
    public int Total { get; set; }

    public bool HasNext { get; set; }

    public IEnumerable<EventResponse> Data { get; set; }
        = Enumerable.Empty<EventResponse>();
}