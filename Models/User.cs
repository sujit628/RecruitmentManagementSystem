namespace RecruitmentManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public string UserType { get; set; } = "Applicant"; // Admin or Applicant
        public string PasswordHash { get; set; } = string.Empty;
        public string ProfileHeadline { get; set; } = string.Empty;

        public Profile? Profile { get; set; }

        // Navigation properties
        public ICollection<Job>? JobsPosted { get; set; } // Jobs posted by this user
        public ICollection<JobApplication>? JobApplications { get; set; } // Applications by this user
    }
}
