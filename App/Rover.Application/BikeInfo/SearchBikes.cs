using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.BikeInfo
{
    public class SearchBikes
    {
        public class Query : IRequest<List<Bike>>
        {
            public string? Brand { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Bike>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Bike>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Bikes.AsQueryable();

                if (!string.IsNullOrEmpty(request.Brand))
                {
                    query = query.Where(bike => bike.Brand == request.Brand);
                }

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
