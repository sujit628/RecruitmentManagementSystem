using System.ComponentModel.DataAnnotations;

public class SignupDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required, MinLength(6)]
    public string Password { get; set; } = null!;
    [Required]
    [RegularExpression("Admin|Applicant")]
    public string UserType { get; set; } = "Applicant";
    public string ProfileHeadline { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
