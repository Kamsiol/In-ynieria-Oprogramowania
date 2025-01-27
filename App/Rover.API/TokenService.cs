using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Rover.Domain;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace Rover.API;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    public string CreateToken(userData user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User object cannot be null.");
        if (string.IsNullOrWhiteSpace(user.nameUser))
            throw new ArgumentNullException(nameof(user.nameUser), "User name cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(user.emailUser))
            throw new ArgumentNullException(nameof(user.emailUser), "User email cannot be null or empty.");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.nameUser),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.emailUser)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
