using Application.Interfaces;

namespace Application.UseCases.Seats.Handlers;

public class UnlockSeatHandler
{
    private readonly ISeatRepository _seatRepository;

    public UnlockSeatHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(Guid seatId, int userId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return false;

        if (seat.LockedByUserId != userId)
            return false;

        seat.Status = "Available";
        seat.LockedByUserId = null;
        seat.LockUntil = null;
        seat.Version++;

        await _seatRepository.UpdateAsync(seat);
        await _seatRepository.SaveChangesAsync();

        return true;
    }
}