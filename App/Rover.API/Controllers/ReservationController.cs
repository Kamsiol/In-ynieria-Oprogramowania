using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rover.Domain;
using Rover.Infrastructure;

namespace Rover.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(DataContext context, ILogger<ReservationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("reserve")]
        public async Task<IActionResult> CreateReservation([FromBody] ReserveRequest request)
        {
            try
            {
                // Проверяем существование пользователя
                var user = await _context.userData.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (user == null)
                {
                    return BadRequest($"User with ID {request.Id} not found.");
                }

                // Проверяем существование велосипеда
                var bike = await _context.bikeData.FirstOrDefaultAsync(b => b.IDbike == request.BikeId);
                if (bike == null)
                {
                    return BadRequest($"Bike with ID {request.BikeId} not found.");
                }

                // Проверяем доступность велосипеда на выбранное время
                var isBikeAvailable = !await _context.orderUser.AnyAsync(o =>
                    o.IDbike == request.BikeId &&
                    o.totalTimeOrderStart < request.EndTime &&
                    o.totalTimeOrderFinish > request.StartTime);

                if (!isBikeAvailable)
                {
                    return BadRequest($"Bike with ID {request.BikeId} is already reserved for the selected time period.");
                }

                // Генерация нового ID для заказа
                int newOrderId = await _context.orderUser.AnyAsync()
                    ? await _context.orderUser.MaxAsync(o => o.IDorder) + 1
                    : 1;

                // Генерация нового ID для платежа
                int newPaymentId = await _context.orderUser.AnyAsync()
                    ? await _context.orderUser.MaxAsync(o => o.IDpayment) + 1
                    : 1;

                // Создаем новую запись аренды
                var newOrder = new orderUser
                {
                    IDorder = newOrderId,
                    Id = request.Id,
                    IDbike = request.BikeId,
                    totalTimeOrderStart = request.StartTime,
                    totalTimeOrderFinish = request.EndTime,
                    IDpayment = newPaymentId // Генерируем ID платежа
                };

                // Сохраняем новую запись в базу данных
                _context.orderUser.Add(newOrder);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Reservation created successfully.", OrderId = newOrder.IDorder, PaymentId = newOrder.IDpayment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation.");
                return StatusCode(500, ex.ToString()); // Для диагностики
            }
        }
    }
}
