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
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewRepository _repository;


        public InterviewController(IInterviewRepository repository)
        {
            _repository = repository;
        }

         //Method for Scheduling Interviews
        [HttpPost("Schedule")]
        public async Task<IActionResult> Schedule([FromBody] Interview input)
        {
            try
            {
                input = await _repository.Schedule(input);
                await _repository.UpdateSlotStatus(input.SlotId);
                return Ok(input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method to cancel the interview
        [HttpPatch("Cancel")]
        public async Task<IActionResult> Cancel( int interviewId)
        {
            try
            {
                await _repository.Cancel(interviewId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method to get feedback of the interview
        [HttpGet("GetFeedback")]
        public async Task<IActionResult> GetFeedback( int interviewId)
        {
            try
            {
                Interview interview = await _repository.GetFeedback(interviewId);
                return Ok(interview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method to get scheduled interviews
        [HttpGet("GetScheduledInterviews")]
        public async Task<IActionResult> GetScheduledInterviews()
        {
            try
            {
                List<Interview> interviews = await _repository.GetScheduledInterviews();
                return Ok(interviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method to filter
        [HttpGet("Filter")]
        public async Task<IActionResult> Filter(string skills, string interviewType)
        {
            try
            {
                List<Interview> interviews = await _repository.Filter(skills,interviewType);
                return Ok(interviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method to filter
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string status)
        {
            try
            {
                List<Interview> interviews = await _repository.Search(status);
                return Ok(interviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}