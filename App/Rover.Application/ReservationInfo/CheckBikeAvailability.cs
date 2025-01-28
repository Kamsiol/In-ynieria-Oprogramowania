using MediatR;
using Microsoft.EntityFrameworkCore;
using Rover.Infrastructure;

namespace Rover.Application.ReservationInfo
{
    public class CheckBikeAvailability
    {
        public class Query : IRequest<bool>
        {
            public required int BikeId { get; set; } // Изменен тип на int
            public required DateTime StartTime { get; set; }
            public required DateTime EndTime { get; set; }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                return !await _context.orderUser
                    .AnyAsync(r => r.IDbike == request.BikeId &&
                                   r.totalTimeOrderStart < request.EndTime &&
                                   r.totalTimeOrderFinish > request.StartTime, cancellationToken);
            }
        }
    }
}
