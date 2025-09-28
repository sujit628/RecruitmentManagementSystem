namespace RecruitmentManagementSystem.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public int ApplicantId { get; set; }
        public User? Applicant { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    }
}