using HiringPortalWebAPI.Data;
using HiringPortalWebAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace HiringPortalWebAPI.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly HiringPortalDbContext _context;
        public ReportRepository(HiringPortalDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Candidate>> GetCandidatesWhoRejectedProposal(int month)
        {
            try
            {
                List<Candidate> candidates = await (from candidate 
                in _context.Candidates join interview in _context.Interviews
                on candidate.Id equals interview.CandidateId
                where interview.Status == "Rejected By Candidate" 
                && interview.InterviewDate.Month.Equals(month) select candidate).ToListAsync();

                return candidates;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Candidate>> GetRejectedCandidates(int month)
        {
            try
            {
                List<Candidate> candidates = await (from candidate 
                in _context.Candidates join interview in _context.Interviews
                on candidate.Id equals interview.CandidateId
                where interview.Status == "Rejected By Interviewer"
                && interview.InterviewDate.Month.Equals(month) select candidate).ToListAsync();

                return candidates;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Candidate>> GetSucceededCandidates(string skills,int month)
        {
            try
            {
                List<Candidate> candidates = await (from candidate 
                in _context.Candidates join interview in _context.Interviews
                on candidate.Id equals interview.CandidateId
                where interview.Status == "Success" && candidate.Skills.Contains(skills)
                && interview.InterviewDate.Month.Equals(month) select candidate).ToListAsync();

                return candidates;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}