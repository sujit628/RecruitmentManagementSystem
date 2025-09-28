using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Models;

namespace RecruitmentManagementSystem.Data
{
    public class RecruitmentDbContext : DbContext
    {
        public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure unique emails
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // -------------------------
            // User → Profile (1:1)
            // -------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: delete profile when user is deleted

            // -------------------------
            // Job → PostedBy (User)
            // -------------------------
            modelBuilder.Entity<Job>()
                .HasOne(j => j.PostedBy)
                .WithMany()
                .HasForeignKey(j => j.PostedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // JobApplication → Applicant (User)
            // -------------------------
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany() // or .WithMany(u => u.JobApplications) if you want reverse nav
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);


            // -------------------------
            // JobApplication → Job
            // -------------------------
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
