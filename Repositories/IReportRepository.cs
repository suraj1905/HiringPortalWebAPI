using HiringPortalWebAPI.Models;

namespace HiringPortalWebAPI.Repositories
{
    public interface IReportRepository
    {
        Task<List<Candidate>> GetRejectedCandidates(int month);

        Task<List<Candidate>> GetCandidatesWhoRejectedProposal(int month);

        Task<List<Candidate>> GetSucceededCandidates(string skills,int month);

    }
}