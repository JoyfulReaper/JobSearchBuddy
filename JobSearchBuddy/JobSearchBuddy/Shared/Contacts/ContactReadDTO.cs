using JobSearchBuddy.Shared.Notes;
using System.ComponentModel.DataAnnotations;

namespace JobSearchBuddy.Shared.Contacts;

public class ContactReadDTO
{
    [Key]
    public int ContactId { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [MaxLength(50)]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Phone]
    [Display(Name = "Phone Number")]
    [MaxLength(12)]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    [Display(Name = "Email Address")]
    [MaxLength(100)]
    public string? EmailAddress { get; set; }

    [Display(Name = "Company Name")]
    [MaxLength(50)]
    public string? CompanyName { get; set; }

    [Display(Name = "Job Title")]
    [MaxLength(50)]
    public string? JobTitle { get; set; }

    [Display(Name = "External Recruiter")]
    public bool IsExternalRecruiter { get; set; }

    [Display(Name = "Date Added")]
    [DataType(DataType.DateTime)]
    public DateTime DateAdded { get; set; }

    [Display(Name = "Date Updated")]
    [DataType(DataType.DateTime)]
    public DateTime? DateUpdated { get; set; }

    public List<NoteReadDto> Notes { get; set; } = new();
}