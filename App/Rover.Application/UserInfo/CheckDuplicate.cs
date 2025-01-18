using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rover.Application.UserInfo
{
    public class CheckDuplicate
    {
        public class Query : IRequest<bool>
        {
            public string? Email { get; set; }
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
                if (string.IsNullOrEmpty(request.Email))
                {
                    throw new ArgumentException("Email must be provided");
                }

                return await _context.Users.AnyAsync(user => user.Email == request.Email, cancellationToken);
            }
        }
    }
}
