using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentManagementSystem.Models
{
    public class Profile
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string? ResumeFile { get; set; }
        public string Skills { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Optional: navigation property
        public User? User { get; set; }
        public User? Applicant { get; set; }
    }
}