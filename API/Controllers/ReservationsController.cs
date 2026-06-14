using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Reservations;
using Application.DTOs;
using Application.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/v1/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly CreateReservationHandler _handler;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;

    // =========================
    // CONSTRUCTOR
    // =========================
    public ReservationsController(
        CreateReservationHandler handler,
        IReservationRepository reservationRepository,
        IUserRepository userRepository)
    {
        _handler = handler;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
    }

    // =========================
    // CREAR RESERVA
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReservationRequest request)
    {
        var result = await _handler.Handle(request);
        return result switch
        {
            ReservationResult.SeatNotFound => NotFound(new { message = "El asiento no existe" }),

            ReservationResult.SeatAlreadyReserved => Conflict(new{ message = "El asiento ya está reservado" }),

            ReservationResult.UserNotFound => NotFound(new { message = "El usuario no existe" }),

            ReservationResult.Success => StatusCode(201, new { message = "Reserva realizada correctamente" }),

            _ => StatusCode(500, new { message = "Error inesperado" })  // Caso Default
        };
    }

    // =========================
    // VER RESERVAS DE USUARIO
    // =========================
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(
        int userId)
    {
        // =========================
        // VALIDAR USUARIO
        // =========================
        var user = await _userRepository.GetByIdAsync(userId);

        // Usuario inexistente
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }

        // =========================
        // OBTENER RESERVAS
        // =========================
        var reservations = await _reservationRepository.GetByUserIdAsync(userId);

        // =========================
        // MAPEAR DTO SIMPLE
        // =========================
        var result = reservations
            .Select(r => new
            {
                id = r.Id,
                seatNumber = r.SeatNumber,
                reservedAt = r.ReservedAt,
                eventName = r.Seat?.Sector?.Event?.Name?? "Evento no encontrado",
                sectorName = r.Seat?.Sector?.Name?? "Sector no encontrado"
            })
            .ToList();

        return Ok(result);
    }
}