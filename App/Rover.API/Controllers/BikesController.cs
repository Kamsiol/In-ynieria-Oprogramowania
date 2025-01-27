using MediatR;
using Rover.Application;
using Rover.Domain;
using Rover.Application.BikeInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Rover.API.Controllers
{
    public class BikesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<bikeData>>> GetBikes()
        {
            var bikes = await _mediator.Send(new BikeList.Query());

            if (bikes == null || bikes.Count == 0)
            {
                return NotFound("No bikes found.");
            }

            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBike(int id) // Изменено Guid -> int
        {
            var result = await _mediator.Send(new BikeInfoOutput.Query { IDbike = id });

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBike(bikeData bike)
        {
            await _mediator.Send(new BikeCreate.Command { bikeData = bike });
            return CreatedAtAction(nameof(GetBike), new { id = bike.IDbike }, bike);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            await _mediator.Send(new BikeDelete.Command { IDbike = id });
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBike(int id, bikeData bike)
        {
            bike.IDbike = id; // Привязка ID
            await _mediator.Send(new BikeModify.Command { bikeData = bike }); // Передаем объект Bike
            return Ok();
        }
    }
}
