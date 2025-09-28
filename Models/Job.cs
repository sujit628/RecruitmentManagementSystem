using Microsoft.AspNetCore.Builder;

namespace RecruitmentManagementSystem.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PostedOn { get; set; } = DateTime.UtcNow;
        public int TotalApplications { get; set; } = 0;
        public string CompanyName { get; set; } = string.Empty;
        public int PostedById { get; set; }
        public User? PostedBy { get; set; }


        public ICollection<JobApplication>? Applications { get; set; }
    }
}