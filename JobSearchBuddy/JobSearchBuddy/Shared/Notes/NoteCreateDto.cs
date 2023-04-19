using System.ComponentModel.DataAnnotations;

namespace JobSearchBuddy.Shared.Notes;

public class NoteCreateDto
{
    [Required]
    [Display(Name = "Note Text")]
    public string NoteText { get; set; } = default!;
}