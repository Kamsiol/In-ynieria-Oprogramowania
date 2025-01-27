using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rover.Domain;
using Rover.Application.ReservationInfo;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Rover.API.Controllers
{
    public class ReservationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IMediator mediator, ILogger<ReservationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveBike(ReserveRequest request)
        {
            try
            {
                // Check bike availability
                var isAvailable = await _mediator.Send(new CheckBikeAvailability.Query
                {
                    IDbike = request.IDbike, // Corrected property name
                    StartTime = request.realTimeOrderStart, // Corrected to use actual start time
                    EndTime = request.realTimeOrderFinish // Corrected to use actual end time
                });

                if (!isAvailable)
                {
                    return BadRequest("The bike is not available for the selected time.");
                }

                // Create a reservation
                await _mediator.Send(new CreateReservation.Command
                {
                    Reservation = new orderUser
                    {
                        IDorder = new Random().Next(1, int.MaxValue), 
                        Id = request.Id,
                        IDbike = request.IDbike,
                        realTimeOrderStart = request.realTimeOrderStart,
                        realTimeOrderFinish = request.realTimeOrderFinish
                    }
                });

                return Ok(new { Message = "Reservation created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
