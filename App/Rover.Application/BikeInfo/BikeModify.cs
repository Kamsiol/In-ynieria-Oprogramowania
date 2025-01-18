using Rover.Domain;
using Rover.Infrastructure;
using Rover.Application;
using MediatR;

namespace Rover.Application.BikeInfo
{
    public class BikeModify
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
                var bike = await _context.Bikes.FindAsync(request.Bike.BikeId);

                bike.Brand = request.Bike.Brand ?? bike.Brand;
                bike.Model = request.Bike.Model ?? bike.Model;
                bike.Type = request.Bike.Type;
                bike.Price = request.Bike.Price;
                bike.Stock = request.Bike.Stock;

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
