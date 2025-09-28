using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.Models;
using System.Security.Claims;


namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly RecruitmentDbContext _db;
        public JobsController(RecruitmentDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _db.Jobs
                .Select(j => new {
                    j.Id,
                    j.Title,
                    j.Description,
                    j.CompanyName,
                    j.PostedOn,
                    j.TotalApplications
                })
                .ToListAsync();
            return Ok(jobs);
        }

        [HttpGet("apply")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> ApplyToJob([FromQuery] int job_id)
        {
            var userIdStr = User.FindFirstValue("userId");
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var job = await _db.Jobs.FindAsync(job_id);
            if (job == null) return NotFound(new { message = "Job not found" });

            var already = await _db.JobApplications.AnyAsync(a => a.JobId == job_id && a.ApplicantId == userId);
            if (already) return BadRequest(new { message = "Already applied" });

            var application = new JobApplication { JobId = job_id, ApplicantId = userId, AppliedOn = DateTime.UtcNow };
            _db.JobApplications.Add(application);
            job.TotalApplications++;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Applied" });
        }
    }


    //public class JobsController : ControllerBase
    //{
    //    private readonly RecruitmentDbContext _db;


    //    public JobsController(RecruitmentDbContext db)
    //    {
    //        _db = db;
    //    }


    //    [HttpGet]
    //    [Authorize]
    //    public async Task<IActionResult> GetJobs()
    //    {
    //        var jobs = await _db.Jobs.ToListAsync();
    //        return Ok(jobs);
    //    }


    //    [HttpGet("apply")]
    //    [Authorize(Roles = "Applicant")]
    //    public async Task<IActionResult> ApplyToJob([FromQuery] int job_id)
    //    {
    //        var userIdStr = User.FindFirstValue("userId");
    //        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();


    //        var job = await _db.Jobs.FindAsync(job_id);
    //        if (job == null) return NotFound(new { message = "Job not found" });


    //        var already = await _db.JobApplications.AnyAsync(a => a.JobId == job_id && a.ApplicantId == userId);
    //        if (already) return BadRequest(new { message = "Already applied" });


    //        var application = new JobApplication { JobId = job_id, ApplicantId = userId };
    //        _db.JobApplications.Add(application);
    //        job.TotalApplications++;
    //        await _db.SaveChangesAsync();


    //        return Ok(new { message = "Applied" });
    //    }
    //}
}