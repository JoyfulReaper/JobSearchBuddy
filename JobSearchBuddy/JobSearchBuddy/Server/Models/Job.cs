namespace JobSearchBuddy.Server.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Url { get; set; }
        public string CompanyName { get; set; } = default!;
        public int? ContactId { get; set; }
        public string? SalaryRange { get; set; }
        public bool IsRemote { get; set; }
        public string? Address1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; } = "PA";
        public string? Zip { get; set; }
        public bool IsInterested { get; set; }
        public bool IsApplied { get; set; }
        public int StatusId { get; set; } = 1;
        public DateTime? DateApplied { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}