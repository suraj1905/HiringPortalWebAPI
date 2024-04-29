using HiringPortalWebAPI.Models;
using HiringPortalWebAPI.Repositories;
using HiringPortalWebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringPortalWebAPI.Controllers
{
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
    }
}