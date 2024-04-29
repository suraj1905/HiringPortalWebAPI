using HiringPortalWebAPI.Data;
using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HiringPortalWebAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HiringPortalDbContext _context;
        public EmployeeRepository(HiringPortalDbContext context) 
        {
            _context = context;
        }
        public async Task<Credential> GetUser(string userId)
        {
            try
            {
                var user = await _context.Credentials.FirstOrDefaultAsync(user => user.UserId == userId);
                return user;
            }
            catch(Exception) 
            {
                throw;
            }
        }


    }
}
