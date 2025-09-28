using System.ComponentModel.DataAnnotations;

public class CreateJobDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    [MaxLength(150)]
    public string CompanyName { get; set; } = string.Empty;
}
