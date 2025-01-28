using MediatR;
using Rover.Domain;
using Rover.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.ReservationInfo
{
    public class CreateReservation
    {
        public class Command : IRequest<Unit>
        {
            public required orderUser Reservation { get; set; }
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
                // Проверяем существование пользователя
                var user = await _context.userData.FirstOrDefaultAsync(u => u.Id == request.Reservation.Id, cancellationToken);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                // Проверяем существование велосипеда
                var bike = await _context.bikeData.FirstOrDefaultAsync(b => b.IDbike == request.Reservation.IDbike, cancellationToken);
                if (bike == null)
                {
                    throw new Exception("Bike not found.");
                }

                if (!bike.availableBike)
                {
                    throw new Exception("Bike is not available.");
                }

                // Генерация нового ID для заказа
                int newOrderId = await _context.orderUser.AnyAsync(cancellationToken)
                    ? await _context.orderUser.MaxAsync(o => o.IDorder, cancellationToken) + 1
                    : 1;

                request.Reservation.IDorder = newOrderId;

                // Сохраняем данные
                await _context.orderUser.AddAsync(request.Reservation, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
