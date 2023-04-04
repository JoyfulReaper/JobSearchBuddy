namespace JobSearchBuddy.Server.Contacts;

public class Contact
{
    public int ContactId { get; set; }
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public bool IsExternalRecruiter { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? DateUpdated { get; set; }
}