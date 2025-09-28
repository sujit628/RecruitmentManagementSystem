using RecruitmentManagementSystem.DTOs;
using RecruitmentManagementSystem.Models;

namespace RecruitmentManagementSystem.Services
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateUserAsync(SignupDto dto);
        Task<User?> ValidateUserAsync(string email, string password);
    }
}
