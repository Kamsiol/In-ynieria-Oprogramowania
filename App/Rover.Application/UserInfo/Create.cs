using Rover.Domain;
using Rover.Infrastructure;
using MediatR;

namespace Rover.Application.UserInfo
{
    public class Create
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
                await _context.Users.AddAsync(request.User, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
