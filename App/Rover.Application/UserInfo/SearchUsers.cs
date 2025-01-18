using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.UserInfo
{
    public class SearchUsers
    {
        public class Query : IRequest<List<User>>
        {
            public string? City { get; set; }
            public string? Email { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(request.City))
                {
                    query = query.Where(user => user.MainCity == request.City);
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query = query.Where(user => user.Email == request.Email);
                }

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
