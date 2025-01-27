using System.Security.Claims;
using Rover.Domain;
using Domain;
using Rover.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rover.API.Controllers;

namespace Cars.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly UserManager<userData> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<userData> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;

    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // szukamy użytkownika w bazie danych, jeśli jest to go pobieramy
        var user = await _userManager.FindByEmailAsync(loginDto.emailUser);

        // jeśli użytkownika nie ma w bazie
        if (user == null) return Unauthorized();

        // zwraca true lub false
        var result = await _userManager.CheckPasswordAsync(user, loginDto.passwordUser);

        if (result)
        {
            return new UserDto
            {
                emailUser = user.emailUser,
                nameUser = user.nameUser,
                Token = _tokenService.CreateToken(user)
            };
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // Проверка на уникальность имени пользователя
        if (await _userManager.Users.AnyAsync(x => x.nameUser == registerDto.nameUser))
        {
            return BadRequest("Username is taken.");
        }

        // Создание нового пользователя
        var user = new userData
        {
            UserName = registerDto.nameUser,
            Email = registerDto.emailUser,
            telnumUser = registerDto.telnumUser,
            nameUser = registerDto.nameUser,
            surnameUser = registerDto.surnameUser,
            birthdayUser = registerDto.birthdayUser,
            sexUser = registerDto.sexUser
        };


        // Попытка создания пользователя
        var result = await _userManager.CreateAsync(user, registerDto.passwordUser);

        // Успешное выполнение
        if (result.Succeeded)
        {
            return new UserDto
            {
                emailUser = user.emailUser,
                nameUser = user.nameUser,
                Token = _tokenService.CreateToken(user)
            };
        }

        // Добавление ошибок в ответ
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest($"Problem with registering user: {string.Join(", ", errors)}");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        return new UserDto
        {
            emailUser = user.emailUser,
            nameUser = user.nameUser,
            Token = _tokenService.CreateToken(user)
        };
    }

}
