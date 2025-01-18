using MediatR;
using Rover.Application;
using Rover.Domain;
using Rover.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rover.Application.UserInfo;
using Rover.Application.BikeInfo;
namespace Rover.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _mediator.Send(new UserList.Query());

            // Проверяем, есть ли пользователи
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }

            // Возвращаем список пользователей
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await Mediator.Send(new UsersInfoOutput.Query { Id = id });

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> EditCar(Guid id, User user)
        {
            user.Id = id;
            await Mediator.Send(new UserModify.Command { User = user });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            await _mediator.Send(new Create.Command { User = user });
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new Delete.Command { Id = id });
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<User>>> SearchUsers(string? city = null, string? email = null)
        {
            var users = await _mediator.Send(new SearchUsers.Query { City = city, Email = email });

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found matching the criteria.");
            }

            return Ok(users);
        }

        [HttpGet("check-duplicate")]
        public async Task<ActionResult<bool>> CheckDuplicate(string email)
        {
            var isDuplicate = await _mediator.Send(new CheckDuplicate.Query { Email = email });
            return Ok(isDuplicate);
        }
    }
}
