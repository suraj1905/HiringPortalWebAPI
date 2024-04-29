using HiringPortalWebAPI.Models;

namespace HiringPortalWebAPI.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdmin(string userName);

        Task CreateJob(Job job);
        Task<Panelist> AddPanelist(Panelist panelist);
        Task AddCandidateProfile(Candidate candidate);

        Task<Credential> CreateCredentials(Credential credential);

        Task<List<Panelist>> GetPanelists();
    }
}
