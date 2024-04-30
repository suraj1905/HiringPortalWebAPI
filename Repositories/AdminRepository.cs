using HiringPortalWebAPI.Data;
using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HiringPortalWebAPI.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HiringPortalDbContext _context;
        public AdminRepository(HiringPortalDbContext context) 
        {
            _context = context;
        }

        public async Task AddCandidateProfile(Candidate candidate)
        {
            try
            {
                await _context.Candidates.AddAsync(candidate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Panelist> AddPanelist(Panelist panelist)
        {
            try
            {
                await _context.Panelists.AddAsync(panelist);
                await _context.SaveChangesAsync();
                return panelist;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Credential> CreateCredentials(Credential credential)
        {
            try
            {
                await _context.Credentials.AddAsync(credential);
                await _context.SaveChangesAsync();
                return credential;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateJob(Job job)
        {
            try
            {
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<Admin> GetAdmin(string userName)
        {
            try
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserName == userName);
                return admin;
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public async Task<List<Candidate>> GetCandidateProfiles()
        {
             try
            {
                return await _context.Candidates.ToListAsync();
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public async Task<List<Job>> GetJobs()
        {
            try
            {
                return await _context.Jobs.ToListAsync();
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public async Task<List<Panelist>> GetPanelists()
        {
            try
            {
                return await _context.Panelists.Include(p=> p.Slots).ToListAsync();
            }
            catch(Exception) 
            {
                throw;
            }
        }
    }
}
