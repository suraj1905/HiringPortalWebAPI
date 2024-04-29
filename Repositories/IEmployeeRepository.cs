using HiringPortalWebAPI.Models;

namespace HiringPortalWebAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Credential> GetUser(string userId);

    }
}