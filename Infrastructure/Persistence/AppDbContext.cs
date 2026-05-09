using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- CONFIGURACIONES TÉCNICAS ---
        modelBuilder.Entity<Sector>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Seat>()
            .Property(s => s.Version)
            .IsConcurrencyToken();

        // RESTRICCIÓN DE UNICIDAD: Evita que existan dos asientos con el mismo número en el mismo sector
        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.SectorId, s.SeatNumber })
            .IsUnique();

        // --- PRECARGA DE DATOS (SEEDING) ---
        
        // 1. Crear el Administrador por defecto
        // NOTA: Usamos una contraseña fija. Si usas BCrypt en el Login, 
        // asegúrate de que el Hash coincida.
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Email = "admin@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("1234"), // O el hash de BCrypt: "$2a$11$Ev9.iP6W.6B9X.xY..."
            Role = "Admin" 
        });

        // 2. Crear un Evento inicial de prueba
        modelBuilder.Entity<Event>().HasData(new Event 
        { 
            Id = 1, 
            Name = "Concierto de Rock Inicial", 
            EventDate = new DateTime(2026, 12, 10, 21, 0, 0, DateTimeKind.Utc), 
            Venue = "Estadio Central", 
            Status = "Active" 
        });

        // 3. Crear los Sectores para ese evento
        modelBuilder.Entity<Sector>().HasData(
            new Sector { Id = 1, EventId = 1, Name = "Platea Baja", Price = 5000, Capacity = 50 },
            new Sector { Id = 2, EventId = 1, Name = "Platea Alta", Price = 8000, Capacity = 50 }
        );

        // 4. Generación automática de los primeros 100 asientos
        var seats = new List<Seat>();
        int[] sectorIds = { 1, 2 };

        foreach (var sId in sectorIds)
        {
            string rowLabel = (sId == 1) ? "Baja" : "Alta";
            int offset = (sId - 1) * 50; 

            for (int i = 1; i <= 50; i++) 
            {
                int seatNumber = i + offset;

                seats.Add(new Seat { 
                    Id = new Guid($"00000000-0000-0000-{sId:D4}-0000{seatNumber:D8}"), 
                    SectorId = sId, 
                    RowIdentifier = rowLabel, 
                    SeatNumber = seatNumber, 
                    Status = "Available", 
                    Version = 1 
                });
            }
        }
        modelBuilder.Entity<Seat>().HasData(seats);
    }
}