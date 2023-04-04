namespace JobSearchBuddy.Server.Notes;

public class Note
{
    public int NoteId { get; set; }
    public string? NoteText { get; set; }
    public string? RelationshipType { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}