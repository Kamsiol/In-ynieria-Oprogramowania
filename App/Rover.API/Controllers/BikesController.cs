using MediatR;
using Rover.Application;
using Rover.Domain;
using Rover.Infrastructure;
using Rover.Application.BikeInfo;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Rover.API.Controllers
{
    public class BikesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bike>>> GetBikes()
        {
            var bikes = await _mediator.Send(new BikeList.Query());

            if (bikes == null || bikes.Count == 0)
            {
                return NotFound("No bikes found.");
            }

            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBike(Guid id)
        {
            var result = await _mediator.Send(new BikeInfoOutput.Query { Id = id });

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBike(Bike bike)
        {
            bike.BikeId = Guid.NewGuid();
            await _mediator.Send(new BikeCreate.Command { Bike = bike });
            return CreatedAtAction(nameof(GetBike), new { id = bike.BikeId }, bike);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(Guid id)
        {
            await _mediator.Send(new BikeDelete.Command { BikeId = id });
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Bike>>> SearchUsers(string? brand = null)
        {
            var bikes = await _mediator.Send(new SearchBikes.Query {Brand = brand});

            if (bikes == null || bikes.Count == 0)
            {
                return NotFound("No bikes found matching the criteria.");
            }

            return Ok(bikes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBike(Guid id, Bike bike)
        {
            bike.BikeId = id;
            await Mediator.Send(new BikeModify.Command { Bike = bike });
            return Ok();
        }
    }
}
