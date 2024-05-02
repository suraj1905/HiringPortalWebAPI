using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Repositories;
using HiringPortalWebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringPortalWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly Utils _utils;


        public EmployeeController(IEmployeeRepository repository,Utils utils)
        {
            _repository = repository;
            _utils = utils;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userId,string password)
        {
            try
            {
                Credential cred = await _repository.GetUser(userId);
                if (cred == null)
                {
                    return BadRequest("User not found");
                }
                if (_utils.VerifyPassword(password, cred.Password))
                {
                    string token = _utils.GenerateToken(cred.PanelistId, cred.UserId, "Employee");
                    var response = new {
                        AccessToken = token,
                        PanelistId = cred.PanelistId
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

         //Method for adding slots
        [HttpPost("AddSlot")]
        public async Task<IActionResult> AddSlot([FromBody] Slot input)
        {
            try
            {
                Slot existingSlot = await _repository.GetSlot(input.DateAvailable,input.TimeAvailable);
                if(existingSlot != null){
                    return BadRequest("Slot Already Exists");
                }
                input.IsBooked = false;
                input = await _repository.AddSlot(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method for editing slots
        [HttpPut("EditSlot")]
        public async Task<IActionResult> EditSlot([FromBody] Slot input)
        {
            try
            {
                Slot existingSlot = await _repository.GetSlot(input.Id);
                if(existingSlot == null){
                    return BadRequest("Slot Doesn't Exist!");
                }
                input = await _repository.EditSlot(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to GetAvailableSlots
        [HttpGet("GetAvailableSlots")]
        public async Task<IActionResult> GetAvailableSlots(int panelistId)
        {
            try
            {
                List<Slot> slots = await _repository.GetAvailableSlots(panelistId);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method for get all slots
        [HttpGet("GetSlots")]
        public async Task<IActionResult> GetSlots(int panelistId)
        {
            try
            {
                List<Slot> slots = await _repository.GetSlots(panelistId);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to get all assignments
        [HttpGet("GetAllAssignments")]
        public async Task<IActionResult> GetAllAssignments(int panelistId)
        {
            try
            {
                List<Interview> interviews = await _repository.GetAllAssignments(panelistId);
                return Ok(interviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to update status of interview
        [HttpPatch("UpdateInterviewStatus")]
        public async Task<IActionResult> UpdateInterviewStatus( int interviewId,[FromBody]string status)
        {
            try
            {
                Interview interview = await _repository.UpdateInterviewStatus(status,interviewId);
                return Ok(interview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to Publish Feedback of Interview
        [HttpPatch("PublishFeedback")]
        public async Task<IActionResult> PublishFeedback( int interviewId, [FromBody] string feedback)
        {
            try
            {
                Interview interview = await _repository.PublishFeedback(feedback,interviewId);
                return Ok(interview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}