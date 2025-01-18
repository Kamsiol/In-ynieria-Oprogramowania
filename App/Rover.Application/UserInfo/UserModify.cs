using Rover.Domain;
using Rover.Infrastructure;
using Rover.Application;
using MediatR;

namespace Rover.Application.UserInfo
{
    public class UserModify
    {
        public class Command : IRequest<Unit>
        {
            public required User User { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.User.Id);

                user.Name = request.User.Name ?? user.Name;
                user.Surname = request.User.Surname ?? user.Surname;
                user.PhoneNunber = request.User.PhoneNunber;
                user.Email = request.User.Email;
                user.MainCity = request.User.MainCity;
                user.Birthday = request.User.Birthday;

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
