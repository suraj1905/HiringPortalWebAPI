using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringPortalWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _repository;

        public ReportController(IReportRepository repository)
        {
            _repository = repository;
        }

         //Method to get rejected candidates
        [HttpGet("GetRejectedCandidates")]
        public async Task<IActionResult> GetRejectedCandidates(int month)
        {
            try
            {
                List<Candidate> candidates = await _repository.GetRejectedCandidates(month);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to get candidates who rejected the proposal
        [HttpGet("GetCandidatesWhoRejectedProposal")]
        public async Task<IActionResult> GetCandidatesWhoRejectedProposal(int month)
        {
            try
            {
                List<Candidate> candidates = await _repository.GetCandidatesWhoRejectedProposal(month);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to get succeeded candidates
        [HttpGet("GetSucceededCandidates")]
        public async Task<IActionResult> GetSucceededCandidates(string skills,int month)
        {
            try
            {
                List<Candidate> candidates = await _repository.GetSucceededCandidates(skills,month);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}