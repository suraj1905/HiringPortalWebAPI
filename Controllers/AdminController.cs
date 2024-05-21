using Azure.Storage.Blobs;
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
        private readonly IConfiguration _configuration;

        private readonly IAdminRepository _repository;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly Utils _utils;


        public AdminController(IAdminRepository repository,Utils utils,IEmployeeRepository employeeRepository,IConfiguration configuration)
        {
            _repository = repository;
            _utils = utils;
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

        //Admin Login
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

        //Method for creating jobs
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

        //Method for adding panelists
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
                return Ok(input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method for adding candidate profiles
        [HttpPost("AddCandidateProfile")]
        public async Task<IActionResult> AddCandidateProfile()
        {
            try
            {
                Candidate candidate = new Candidate();
                var formCollection = await Request.ReadFormAsync();
                candidate.Name = Request.Form["name"]!;
                candidate.Email = Request.Form["email"]!;
                candidate.PhoneNo = Int64.Parse(Request.Form["phoneNo"]!);
                candidate.YearsOfExperience = Decimal.Parse(Request.Form["yearsOfExperience"]!);
                candidate.Skills = Request.Form["skills"]!;
                var ResumeFile = formCollection.Files.First();
                var fileName = _utils.GenerateFileName(ResumeFile.FileName, ResumeFile.Name);
                BlobContainerClient containerClient = new BlobContainerClient(_configuration["Blob:ConnectionStrings"],_configuration["Blob:ContainerName"]);
                try
                { 
                    BlobClient blobClient = containerClient.GetBlobClient(fileName);
                    using(Stream stream = ResumeFile.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream);
                    }
                    candidate.Resume = blobClient.Uri.AbsoluteUri;
                }
                catch(Exception ex){
                    throw;
                }
                await _repository.AddCandidateProfile(candidate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to get all panelists
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

        //Method to get all panelists
        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                List<Job> jobs = await _repository.GetJobs();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //Method to get all panelists
        [HttpGet("GetCandidateProfiles")]
        public async Task<IActionResult> GetCandidateProfiles()
        {
            try
            {
                List<Candidate> candidates = await _repository.GetCandidateProfiles();
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
