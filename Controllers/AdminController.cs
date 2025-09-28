using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.DTOs;
using RecruitmentManagementSystem.Models;
using System.Security.Claims;


namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly RecruitmentDbContext _db;
        public AdminController(RecruitmentDbContext db)
        {
            _db = db;
        }
        // POST /admin/job
        [HttpPost("job")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdStr = User.FindFirstValue("userId");
            int.TryParse(userIdStr, out var postedBy);

            var job = new Job
            {
                Title = dto.Title,
                Description = dto.Description,
                CompanyName = dto.CompanyName,
                PostedOn = DateTime.UtcNow,
                PostedById = postedBy
            };

            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobWithApplicants), new { id = job.Id }, job);
        }

        [HttpGet("job/{id}")]
        public async Task<IActionResult> GetJobWithApplicants(int id)
        {
            var job = await _db.Jobs
                .Include(j => j.Applications)
                .ThenInclude(a => a.Applicant)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null) return NotFound();

            var applicants = job.Applications?.Select(a => new ApplicantSummaryDto
            {
                Id = a.Applicant!.Id,
                Name = a.Applicant.Name,
                Email = a.Applicant.Email,
                ProfileHeadline = a.Applicant.ProfileHeadline,
                Skills = a.Applicant.Profile?.Skills ?? string.Empty
            });

            return Ok(new { job, applicants });
        }

        [HttpGet("applicants")]
        public async Task<IActionResult> GetAllApplicants()
        {
            var users = await _db.Users
                .Where(u => u.UserType == "Applicant")
                .Include(u => u.Profile)
                .Select(u => new ApplicantSummaryDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    ProfileHeadline = u.ProfileHeadline,
                    Skills = u.Profile != null ? u.Profile.Skills : string.Empty
                }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("applicant/{id}")]
        public async Task<IActionResult> GetApplicant(int id)
        {
            var user = await _db.Users
                .Where(u => u.Id == id && u.UserType == "Applicant")
                .Include(u => u.Profile)
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Address,
                user.ProfileHeadline,
                Profile = user.Profile // includes Skills/Education/Experience/ResumeFile
            });
        }
    }


    //    public class AdminController : ControllerBase
    //    {
    //        private readonly RecruitmentDbContext _db;
    //        public AdminController(RecruitmentDbContext db)
    //        {
    //            _db = db;
    //        }


    //        [HttpPost("job")]
    //        public async Task<IActionResult> CreateJob([FromBody] Job job)
    //        {
    //            // PostedById should be set from token if needed
    //            _db.Jobs.Add(job);
    //            await _db.SaveChangesAsync();
    //            return Ok(job);
    //        }


    //        [HttpGet("job/{id}")]
    //        public async Task<IActionResult> GetJobWithApplicants(int id)
    //        {
    //#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    //            Job? job = await _db.Jobs
    //            .Include(j => j.Applications).ThenInclude(a => a.Applicant).FirstOrDefaultAsync(j => j.Id == id);
    //#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.


    //            if (job == null) return NotFound();


    //            var applicants = job.Applications?.Select(a => new {
    //                a.ApplicantId,
    //                a.Applicant?.Name,
    //                a.Applicant?.Email,
    //                AppliedOn = a.AppliedOn
    //            });


    //            return Ok(new { job, applicants });
    //        }


    //        [HttpGet("applicants")]
    //        public async Task<IActionResult> GetAllApplicants()
    //        {
    //            var users = await _db.Users.Include(u => u.Profile).ToListAsync();
    //            return Ok(users);
    //        }


    //        [HttpGet("applicant/{id}")]
    //        public async Task<IActionResult> GetApplicant(int id)
    //        {
    //            var user = await _db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == id);
    //            if (user == null) return NotFound();
    //            return Ok(user);
    //        }
    //    }
}