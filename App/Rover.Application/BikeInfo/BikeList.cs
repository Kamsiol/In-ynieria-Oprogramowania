using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.BikeInfo
{
    public class BikeList
    {
        public class Query : IRequest<List<Bike>> { }

        public class Handler : IRequestHandler<Query, List<Bike>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Bike>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Bikes.ToListAsync(cancellationToken);
            }
        }
    }
}
