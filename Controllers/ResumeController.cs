using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.Models;
using RecruitmentManagementSystem.Services;
using System.Security.Claims;

namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly RecruitmentDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IThirdPartyParser _parser; // add service for parser

        public ResumeController(RecruitmentDbContext db, IWebHostEnvironment env, IThirdPartyParser parser)
        {
            _db = db;
            _env = env;
            _parser = parser;
        }

        [HttpPost("uploadResume")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".pdf" && ext != ".docx") return BadRequest(new { message = "Only PDF or DOCX allowed" });

            var userIdStr = User.FindFirstValue("userId");
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            // store under wwwroot/resumes/{userId}/
            var relFolder = Path.Combine("resumes", userId.ToString());
            var absFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", relFolder);
            Directory.CreateDirectory(absFolder);

            var fileName = $"resume_{DateTime.UtcNow:yyyyMMddHHmmss}_{Path.GetRandomFileName()}{ext}";
            var absPath = Path.Combine(absFolder, fileName);
            var relPath = Path.Combine(relFolder, fileName).Replace("\\", "/");

            await using (var fs = System.IO.File.Create(absPath))
            {
                await file.CopyToAsync(fs);
            }

            // Save relative path to DB
            var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
            {
                profile = new Profile { UserId = userId, ResumeFile = relPath };
                _db.Profiles.Add(profile);
            }
            else
            {
                profile.ResumeFile = relPath;
            }
            await _db.SaveChangesAsync();

            // Asynchronously send to parser (do not block main thread on long requests)
            _ = Task.Run(async () =>
            {
                try
                {
                    var parsed = await _parser.ParseResumeAsync(absPath); // returns object with Skills, Education, Experience, Phone, Email
                    if (parsed != null)
                    {
                        profile.Skills = parsed.Skills ?? profile.Skills;
                        profile.Education = parsed.Education ?? profile.Education;
                        profile.Experience = parsed.Experience ?? profile.Experience;
                        profile.Phone = parsed.Phone ?? profile.Phone;
                        await _db.SaveChangesAsync();
                    }
                }
                catch
                {
                    // log error (do not throw)
                }
            });

            return Ok(new { message = "Uploaded", resume = relPath });
        }
    }

}
