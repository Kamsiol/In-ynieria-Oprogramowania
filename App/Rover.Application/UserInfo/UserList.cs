using Rover.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Rover.Infrastructure
{
    public class UserList
    {
        private readonly DataContext _context;

        public UserList(DataContext context)
        {
            _context = context;
        }

        public async Task<List<userData>> GetAllUsersAsync()
        {
            return await _context.userData.ToListAsync();
        }
    }
}
