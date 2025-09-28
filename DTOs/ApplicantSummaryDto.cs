namespace RecruitmentManagementSystem.DTOs
{
    public class ApplicantSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfileHeadline { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
    }

}
