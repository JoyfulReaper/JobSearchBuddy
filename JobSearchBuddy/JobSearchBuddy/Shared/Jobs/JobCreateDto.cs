using System.ComponentModel.DataAnnotations;

namespace JobSearchBuddy.Shared.Jobs;

public class JobCreateDto
{
    [Required]
    [MaxLength(50)]
    [Display(Name = "Job Title")]
    public string Title { get; set; } = default!;

    [Display(Name = "Job Description")]
    [Required]
    public string Description { get; set; } = default!;

    [Display(Name = "Job URL")]
    [MaxLength(150)]
    public string? Url { get; set; }

    [Display(Name = "Company Name")]
    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; } = default!;

    [Display(Name = "Contact")]
    public int? ContactId { get; set; }

    [Display(Name = "Salary Range")]
    [MaxLength(50)]
    public string? SalaryRange { get; set; }

    [Display(Name = "Remote")]
    public bool IsRemote { get; set; }

    [Display(Name = "Address 1")]
    [MaxLength(100)]
    public string? Address1 { get; set; }

    [MaxLength(50)]
    [Display(Name = "City")]
    public string? City { get; set; }

    [MaxLength(2)]
    [Display(Name = "State")]
    public string? State { get; set; } = "PA";

    [MaxLength(10)]
    [Display(Name = "Zip")]
    public string? Zip { get; set; }

    [Display(Name = "Interested")]
    public bool IsInterested { get; set; }

    [Display(Name = "Applied")]
    public bool IsApplied { get; set; }

    [Display(Name = "Status")]
    public int StatusId { get; set; } = 1;

    [Display(Name = "Date Applied")]
    [DataType(DataType.DateTime)]
    public DateTime? DateApplied { get; set; }

    [Display(Name = "Date Posted")]
    [DataType(DataType.Date)]
    public DateTime? DatePosted { get; set; }
}