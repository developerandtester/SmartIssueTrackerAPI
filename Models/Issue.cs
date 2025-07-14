namespace SmartIssueTrackerAPI.Models
{
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public string Assignee { get; set; } = string.Empty;
        public List<Comment> Comments { get; set; } = new();
    }

    
}