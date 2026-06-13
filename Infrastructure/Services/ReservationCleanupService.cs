using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services;

public class ReservationCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReservationCleanupService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var context =
                    scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

                var expiredSeats =
                    await context.Seats
                        .Where(s =>
                            s.Status == "Reserved" &&
                            s.LockUntil != null &&
                            s.LockUntil < DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                foreach (var seat in expiredSeats)
                {
                    seat.Status = "Available";
                    seat.LockedByUserId = null;
                    seat.LockUntil = null;
                    seat.Version++;
                }

                if (expiredSeats.Count > 0)
                {
                    await context.SaveChangesAsync(
                        stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Cleanup Error: {ex.Message}");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }
}