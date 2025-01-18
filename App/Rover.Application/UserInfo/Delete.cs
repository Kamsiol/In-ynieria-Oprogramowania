using Rover.Domain;
using Rover.Infrastructure;
using MediatR;

namespace Rover.Application.UserInfo
{
    public class Delete
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
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
                var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);

                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                _context.Users.Remove(user);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
