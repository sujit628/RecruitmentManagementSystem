namespace RecruitmentManagementSystem.DTOs
{
    public record SignupDto(string Name, string Email, string Password, string UserType, string ProfileHeadline, string Address);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string TokenType = "Bearer");
}