using Application.UseCases.Events.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.UseCases.Events.Queries;
using Microsoft.EntityFrameworkCore;
using Application.UseCases.Auth;
using Application.UseCases.Reservations; // Asegurate de que esta ruta sea la correcta para CreateReservationHandler

var builder = WebApplication.CreateBuilder(args);

// ==========================================================================
// 1. CONFIGURACIÓN DE SERVICIOS BÁSICOS
// ==========================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================================================
// 2. CONFIGURACIÓN DE LA BASE DE DATOS (PostgreSQL)
// ==========================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================================================
// 3. INYECCIÓN DE DEPENDENCIAS (Inyección de Repositorios y Handlers)
// ==========================================================================

// --- REPOSITORIOS (Infrastructure) ---
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// --- CASOS DE USO / HANDLERS (Application) ---

// Gestión de Eventos
builder.Services.AddScoped<GetEventsHandler>();
builder.Services.AddScoped<GetSeatsBySectorHandler>();
builder.Services.AddScoped<GenerateEventHandler>(); // El motor de creación de asientos

// Autenticación
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RegisterHandler>();

// Reservas
builder.Services.AddScoped<CreateReservationHandler>();

// ==========================================================================
// 4. CONFIGURACIÓN DE CORS
// ==========================================================================
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => 
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ==========================================================================
// 5. CONFIGURACIÓN DEL PIPELINE DE LA APP (Middleware)
// ==========================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Importante: UseCors debe ir antes de UseAuthorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();