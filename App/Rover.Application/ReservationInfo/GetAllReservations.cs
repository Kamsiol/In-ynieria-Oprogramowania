using MediatR;
using Microsoft.EntityFrameworkCore;
using Rover.Domain;
using Rover.Infrastructure;

namespace Rover.Application.ReservationInfo
{
    public class GetAllReservations
    {
        public class Query : IRequest<List<orderUser>> { }

        public class Handler : IRequestHandler<Query, List<orderUser>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<orderUser>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Загружаем все данные о заказах
                var orders = await _context.orderUser
                    .Include(o => o.Id) // Подгружаем связанные данные, если нужно
                    .Include(o => o.IDbike) // Например, информацию о велосипеде
                    .ToListAsync(cancellationToken);

                return orders;
            }
        }
    }
}
