using Rover.Domain;
using Rover.Infrastructure;
using MediatR;
using Rover.Application.UserInfo;
using Microsoft.EntityFrameworkCore;
using Rover.Application;

namespace Rover.Application.UserInfo
{
    public class UsersInfoOutput
    {
        public class Query : IRequest<Result<User>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<UsersInfoOutput.Query, Result<User>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<User>> Handle(UsersInfoOutput.Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.Id);

                if (user == null)
                {
                    return Result<User>.Failure("User not found");
                }

                return Result<User>.Success(user);
            }
        }
    }
}
