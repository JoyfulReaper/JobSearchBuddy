using System.ComponentModel.DataAnnotations;

namespace JobSearchBuddy.Shared.Notes;

public class NoteReadDto
{
    [Key]
    public int NoteId { get; set; }

    [Required]
    [Display(Name = "Note Text")]
    public string NoteText { get; set; } = default!;

    [Display(Name = "Relationship Type")]
    public string? RelationshipType { get; set; }

    [Required]
    [Display(Name = "Date Created")]
    [DataType(DataType.DateTime)]
    public DateTime DateCreated { get; set; }

    [Display(Name = "Date Updated")]
    [DataType(DataType.DateTime)]
    public DateTime? DateUpdated { get; set; }
}