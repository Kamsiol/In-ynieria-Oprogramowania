using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rover.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.Users
{
    public class LoginUser
    {
        // Запрос
        public class Query : IRequest<string>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // Обработчик запроса
        public class Handler : IRequestHandler<Query, string>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                // Проверяем пользователя
                var user = await _context.userData
                    .FirstOrDefaultAsync(u => u.emailUser == request.Email && u.passwordUser == request.Password, cancellationToken);

                if (user == null)
                {
                    return "Invalid email or password.";
                }

                // Возвращаем id, email и пароль
                return $"Login successful. ID: {user.Id}, Email: {user.emailUser}, Password: {user.passwordUser}";
            }
        }
    }
}
