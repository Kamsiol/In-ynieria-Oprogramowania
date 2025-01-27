using System.Text;
using Rover.Domain;
using Rover.Infrastructure;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Rover.API;

public static class IdentityService
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _config)
    {
        services.AddIdentityCore<userData>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<DataContext>();

        // klucz musi być identyczny jak klucz szyfrujący w TokenService
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(/*"super tajne haslol23!!$super tajne haslol23!!$super tajne haslol23!!$super tajne haslol23!!$"*/_config["TokenKey"]));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            // jak walidować token
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // Walidacja klucza podpisującego token
                IssuerSigningKey = key, // Użycie wcześniej zdefiniowanego klucza symetrycznego do weryfikacji tokenu
                ValidateIssuer = false, // Nie walidujemy wystawcy tokenu (Issuer)
                ValidateAudience = false // Nie walidujemy odbiorcy tokenu (Audience)
            };
        });

        // dodanie serwisu odpowiedzialnego za tworzenie tokenów
        services.AddScoped<TokenService>();

        return services;
    }
}
