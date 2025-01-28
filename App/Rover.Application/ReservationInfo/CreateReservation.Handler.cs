//using MediatR;
//using Rover.Application.ReservationInfo;
//using Rover.Infrastructure;
//using Microsoft.EntityFrameworkCore;

//public class Handler : IRequestHandler<CreateReservation.Command, Unit>
//{
//    private readonly DataContext _context;

//    public Handler(DataContext context)
//    {
//        _context = context;
//    }

//    public async Task<Unit> Handle(CreateReservation.Command request, CancellationToken cancellationToken)
//    {
//        // Проверяем корректность BikeId
//        if (!Guid.TryParse(request.Reservation.BikeId, out var bikeIdGuid))
//        {
//            throw new ArgumentException("Invalid BikeId format.");
//        }

//        // Проверяем существование велосипеда и получаем его цену
//        var bike = await _context.Bikes.FirstOrDefaultAsync(b => b.BikeId == bikeIdGuid, cancellationToken);
//        if (bike == null)
//        {
//            throw new Exception("Bike not found.");
//        }

//        // Рассчитываем количество дней аренды
//        var rentalDays = (request.Reservation.EndTime.Date - request.Reservation.StartTime.Date).Days;
//        if (rentalDays <= 0)
//        {
//            throw new ArgumentException("EndTime must be at least one day after StartTime.");
//        }

//        // Рассчитываем общую стоимость
//        request.Reservation.TotalCost = Math.Round((decimal)rentalDays * (decimal)bike.Price, 2);

//        // Сохраняем бронирование
//        await _context.Reservations.AddAsync(request.Reservation, cancellationToken);
//        await _context.SaveChangesAsync(cancellationToken);

//        return Unit.Value;
//    }
//}
