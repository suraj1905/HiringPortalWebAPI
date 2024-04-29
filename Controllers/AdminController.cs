using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Repositories;
using HiringPortalWebAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringPortalWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repository;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly Utils _utils;


        public AdminController(IAdminRepository repository,Utils utils,IEmployeeRepository employeeRepository)
        {
            _repository = repository;
            _utils = utils;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName,string password)
        {
            try
            {
                Admin admin = await _repository.GetAdmin(userName);
                if (admin == null)
                {
                    return BadRequest("Admin doesn't exist.");
                }
                if (_utils.VerifyPassword(password, admin.Password))
                {
                    string token = _utils.GenerateToken(admin.Id, admin.UserName, "Admin");
                    var response = new {
                        AccessToken = token
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

        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob([FromBody] Job jobInput)
        {
            try
            {
                await _repository.CreateJob(jobInput);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("AddPanelist")]
        public async Task<IActionResult> AddPanelist([FromBody] Panelist input)
        {
            try
            {
                Credential existing = await _employeeRepository.GetUser(input.Email);
                if(existing != null)
                {
                    return BadRequest("Panelist Already Exists");
                }
                input = await _repository.AddPanelist(input);
                string password = _utils.GeneratePassword();
                Credential cred = new Credential
                {
                    UserId = input.Email,
                    Password = _utils.HashPassword(password),
                    PanelistId = input.Id
                };
                cred = await _repository.CreateCredentials(cred);
                _utils.SendEmail(cred,password);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("AddCandidateProfile")]
        public async Task<IActionResult> AddCandidateProfile([FromBody] Candidate input)
        {
            try
            {
                await _repository.AddCandidateProfile(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetAllPanelists")]
        public async Task<IActionResult> GetAllPanelists()
        {
            try
            {
                List<Panelist> panelists = await _repository.GetPanelists();
                return Ok(panelists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
