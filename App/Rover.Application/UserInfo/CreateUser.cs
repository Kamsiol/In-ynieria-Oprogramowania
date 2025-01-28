using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rover.Domain;
using Rover.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.Users
{
    public class CreateUser
    {
        // Команда
        public class Command : IRequest<string>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int PhoneNumber { get; set; }
            public DateTime Birthday { get; set; }
            public string Gender { get; set; }
        }

        // Обработчик команды
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                // Проверяем, существует ли пользователь с таким email
                var existingUser = await _context.userData
                    .FirstOrDefaultAsync(u => u.emailUser == request.Email, cancellationToken);

                if (existingUser != null)
                {
                    return "User with this email already exists.";
                }

                // Генерация нового Id
                int newId = await _context.userData.AnyAsync(cancellationToken)
                    ? await _context.userData.MaxAsync(u => u.Id, cancellationToken) + 1
                    : 1;

                // Создаем нового пользователя с вручную установленным Id
                var newUser = new userData
                {
                    Id = newId,
                    emailUser = request.Email,
                    passwordUser = request.Password,
                    nameUser = request.Name,
                    surnameUser = request.Surname,
                    telnumUser = request.PhoneNumber,
                    birthdayUser = request.Birthday,
                    sexUser = request.Gender
                };

                _context.userData.Add(newUser);
                await _context.SaveChangesAsync(cancellationToken);

                return $"User created successfully with ID: {newUser.Id}";
            }
        }
    }
}
