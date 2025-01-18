using MediatR;
using Rover.Domain;
using Rover.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Application.BikeInfo
{
    public class BikeInfoOutput
    {
        public class Query : IRequest<Result<Bike>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<BikeInfoOutput.Query, Result<Bike>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Bike>> Handle(BikeInfoOutput.Query request, CancellationToken cancellationToken)
            {
                var bike = await _context.Bikes.FindAsync(request.Id);

                if (bike == null)
                {
                    return Result<Bike>.Failure("Bike not found");
                }

                return Result<Bike>.Success(bike);
            }
        }
    }
}
