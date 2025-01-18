using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.BikeInfo
{
    public class BikeDelete
    {
        public class Command : IRequest<Unit>
        {
            public Guid BikeId { get; set; }
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
                var bike = await _context.Bikes.FindAsync(new object[] { request.BikeId }, cancellationToken);

                if (bike == null)
                {
                    throw new Exception("Bike not found");
                }

                _context.Bikes.Remove(bike);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
