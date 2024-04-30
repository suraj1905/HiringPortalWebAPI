using HiringPortalWebAPI.Data;
using HiringPortalWebAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace HiringPortalWebAPI.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly HiringPortalDbContext _context;
        public InterviewRepository(HiringPortalDbContext context) 
        {
            _context = context;
        }

        public async Task Cancel(int interviewId)
        {
            Interview interview = await _context.Interviews.FindAsync(interviewId);
            interview.Status = "Cancelled";
            _context.Entry(interview).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Interview>> Filter(string skills, string interviewType)
        {
            return await (from interview in _context.Interviews join job in _context.Jobs
                on interview.JobId equals job.Id
                where job.Description.Contains(skills) && interview.InterviewType == interviewType
                select interview).Include(i=> i.Job).Include(i=> i.Candidate).Include(i=> i.Panelist).ToListAsync();
        }

        public async Task<Interview> GetFeedback(int interviewId)
        {
            try
            {
                return await _context.Interviews.FindAsync(interviewId);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Interview>> GetScheduledInterviews()
        {
            try
            {
                return await _context.Interviews.Include(i=> i.Candidate).Include(i=> i.Job)
                .Include(i=>i.Panelist).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Interview> Schedule(Interview interview)
        {
            try
            {
                await _context.Interviews.AddAsync(interview);
                await _context.SaveChangesAsync();
                return interview;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Interview>> Search(string status)
        {
            try
            {
                return await _context.Interviews.Where(i => i.Status == status).Include(i=> i.Job)
                .Include(i=>i.Candidate).Include(i=>i.Panelist).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateSlotStatus(int slotId)
        {
            try
            {
                Slot slot = await _context.Slots.FirstOrDefaultAsync(s => s.Id == slotId);
                slot.IsBooked = true;
                _context.Entry(slot).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}