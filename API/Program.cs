using API.Middleware;
using Application.Interfaces;
using Application.UseCases.Auth;
using Application.UseCases.Events.Handlers;
using Application.UseCases.Events.Queries;
using Application.UseCases.Reservations;
using Application.UseCases.Seats.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================================
// SERVICIOS BÁSICOS
// ==========================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================================================
// BASE DE DATOS
// ==========================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================================================
// REPOSITORIOS
// ==========================================================================
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// ==========================================================================
// HANDLERS
// ==========================================================================

// Eventos
builder.Services.AddScoped<GetEventsHandler>();
builder.Services.AddScoped<GetSeatsBySectorHandler>();
builder.Services.AddScoped<GenerateEventHandler>();

// Auth
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RegisterHandler>();

// Reservas
builder.Services.AddScoped<CreateReservationHandler>();

// Asientos
builder.Services.AddScoped<LockSeatHandler>();
builder.Services.AddScoped<UnlockSeatHandler>();

// Servicios
builder.Services.AddHostedService<ReservationCleanupService>();

// ==========================================================================
// CORS
// ==========================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ==========================================================================
// PIPELINE
// ==========================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();