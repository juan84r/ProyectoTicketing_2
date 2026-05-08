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

    // Inyectamos el handler para crear y el repositorio para leer
    public ReservationsController(CreateReservationHandler handler, IReservationRepository reservationRepository)
    {
        _handler = handler;
        _reservationRepository = reservationRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationRequest request)
    {
        var result = await _handler.Handle(request);

        return result switch
        {
            ReservationResult.SeatNotFound => NotFound("El asiento no existe"),
            ReservationResult.SeatAlreadyReserved => Conflict("El asiento ya está reservado"),
            ReservationResult.UserNotFound => NotFound("El usuario no existe"),
            ReservationResult.Success => Created("", "Reserva realizada correctamente"),
            _ => StatusCode(500, "Error inesperado")
        };
    }

    // Endpoint que llama tu Frontend para "Ver mis reservas"
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var reservations = await _reservationRepository.GetByUserIdAsync(userId);
        return Ok(reservations);
    }
}