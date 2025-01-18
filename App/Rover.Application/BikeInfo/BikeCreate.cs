using Rover.Domain;
using Rover.Infrastructure;
using MediatR;

namespace Rover.Application.BikeInfo
{
    public class BikeCreate
    {
        public class Command : IRequest<Unit>
        {
            public required Bike Bike { get; set; }
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
                await _context.Bikes.AddAsync(request.Bike, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
