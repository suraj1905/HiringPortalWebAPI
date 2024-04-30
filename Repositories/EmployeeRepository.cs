using HiringPortalWebAPI.Data;
using HiringPortalWebAPI.Models;

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

        public async Task<Slot> AddSlot(Slot slot)
        {
            try
            {
                await _context.Slots.AddAsync(slot);
                await _context.SaveChangesAsync();
                return slot;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Slot> EditSlot(Slot slot)
        {
            try
            {
                Slot existingSlot = await _context.Slots.FindAsync(slot.Id);
                existingSlot.DateAvailable = slot.DateAvailable;
                existingSlot.TimeAvailable = slot.TimeAvailable;
                _context.Entry(existingSlot).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return slot;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Slot> GetSlot(DateOnly date, TimeOnly time)
        {
            try
            {
                Slot slot =  await _context.Slots.FirstOrDefaultAsync(s => s.DateAvailable == date && s.TimeAvailable == time);
                return slot;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         public async Task<Slot> GetSlot(int id)
        {
            try
            {
                Slot slot =  await _context.Slots.FindAsync(id);
                return slot;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Interview>> GetAllAssignments(int panelistId)
        {
            try
            {
                List<Interview> interviews = await _context.Interviews.Where(interview => interview.PanelistId == panelistId)
                .Include(i=> i.Candidate).Include(i=> i.Job).ToListAsync();
                return interviews;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Interview> UpdateInterviewStatus(string status, int interviewId)
        {
            try
            {
                Interview interview = await _context.Interviews.FindAsync(interviewId);
                interview.Status = status;
                _context.Entry(interview).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return interview;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Interview> PublishFeedback(string feedback, int interviewId)
        {
            try
            {
                Interview interview = await _context.Interviews.FindAsync(interviewId);
                interview.Feedback = feedback;
                _context.Entry(interview).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return interview;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Slot>> GetAvailableSlots(int panelistId)
        {
            try
            {

                return await _context.Slots.Where(slot => slot.PanelistId == panelistId && slot.IsBooked == false).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Slot>> GetSlots(int panelistId)
        {
            try
            {

                return await _context.Slots.Where(slot => slot.PanelistId == panelistId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
