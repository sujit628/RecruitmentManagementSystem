using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.DTOs;
using RecruitmentManagementSystem.Models;

namespace RecruitmentManagementSystem.Services
{
    public class UserService : IUserService   // 👈 must implement IUserService
    {
        private readonly RecruitmentDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public UserService(RecruitmentDbContext db)
        {
            _db = db;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.Include(u => u.Profile)
                                  .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(SignupDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                UserType = dto.UserType,
                ProfileHeadline = dto.ProfileHeadline
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var profile = new Profile { UserId = user.Id };
            _db.Profiles.Add(profile);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}
