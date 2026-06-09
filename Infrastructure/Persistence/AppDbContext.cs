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

        // --- CONFIGURACIONES TECNICAS ---
        modelBuilder.Entity<Sector>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Seat>()
            .Property(s => s.Version)
            .IsConcurrencyToken();

        // RESTRICCION DE UNICIDAD: Evita que existan dos asientos con el mismo numero en el mismo sector
        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.SectorId, s.SeatNumber })
            .IsUnique();

        // --- PRECARGA DE DATOS (SEEDING) ---
        
        // 1. Crear el Administrador por defecto
        // NOTA: Usamos una contraseña fija. Si usas BCrypt en el Login
        
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Email = "admin@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("1234"), // O el hash de BCrypt: "$2a.."
            Role = "Admin" 
        });

        
        // --- PRECARGA DE DATOS (SEEDING) ---

// 1. Evento
var eventoInicial = new Event 
{ 
    Id = 1, 
    Name = "Concierto de Rock", 
    EventDate = new DateTime(2026, 12, 10, 21, 0, 0, DateTimeKind.Utc), 
    Venue = "Estadio Principal", 
    Status = "Active" 
};
modelBuilder.Entity<Event>().HasData(eventoInicial);

// 2. Sectores
var sectores = new List<Sector>
{
    new Sector { Id = 1, EventId = 1, Name = "Sector A", Price = 5000, Capacity = 50 },
    new Sector { Id = 2, EventId = 1, Name = "Sector B", Price = 8000, Capacity = 50 }
};
modelBuilder.Entity<Sector>().HasData(sectores);

// 3. Asientos (Lógica identica al Handler)
var seats = new List<Seat>();
foreach (var sector in sectores)
{
    // EL CONTADOR SE RESETEA PARA CADA SECTOR
    int contadorAsiento = 1; 

    for (int i = 1; i <= 50; i++) 
    {
        seats.Add(new Seat 
        { 
            // El ID debe ser unico, usamos el sector.Id para diferenciar
            Id = new Guid($"00000000-0000-0000-{sector.Id:D4}-{contadorAsiento:D12}"), 
            SectorId = sector.Id, 
            RowIdentifier = sector.Name.Substring(sector.Name.Length - 1), 
            SeatNumber = contadorAsiento, 
            Status = "Available", 
            Version = 1 
        });
        contadorAsiento++;
    }
}
modelBuilder.Entity<Seat>().HasData(seats);
    }
}