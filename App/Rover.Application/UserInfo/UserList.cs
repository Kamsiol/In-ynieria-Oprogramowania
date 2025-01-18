using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.UserInfo
{
    public class UserList
    {
        public class Query : IRequest<List<User>> { }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Users.ToListAsync();
            }
        }
    }
}