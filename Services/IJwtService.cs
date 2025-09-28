namespace RecruitmentManagementSystem.Services
{
    using RecruitmentManagementSystem.Models;


    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}