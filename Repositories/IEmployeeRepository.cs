using HiringPortalWebAPI.Models;

namespace HiringPortalWebAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Credential> GetUser(string userId);

        Task<Slot> AddSlot(Slot slot);

        Task<Slot> EditSlot(Slot slot);

        Task<Slot> GetSlot(DateOnly date, TimeOnly time);

        Task<Slot> GetSlot(int id);

        Task<List<Slot>> GetSlots(int panelistId);

        Task<List<Slot>> GetAvailableSlots(int panelistId);

        Task<List<Interview>> GetAllAssignments(int panelistId);

        Task<Interview> UpdateInterviewStatus(string status, int interviewId);

        Task<Interview> PublishFeedback(string feedback, int interviewId);
    }
}