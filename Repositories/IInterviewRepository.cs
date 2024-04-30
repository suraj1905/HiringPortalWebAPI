using HiringPortalWebAPI.Models;

namespace HiringPortalWebAPI.Repositories
{
    public interface IInterviewRepository
    {
        Task<Interview> Schedule(Interview interview);

        Task Cancel(int interviewId);

        Task UpdateSlotStatus(int slotId);

        Task<Interview> GetFeedback(int interviewId);

        Task<List<Interview>> Filter(string skill, string interviewType);

        Task<List<Interview>> Search(string status);
        
        Task<List<Interview>> GetScheduledInterviews();
    }
}