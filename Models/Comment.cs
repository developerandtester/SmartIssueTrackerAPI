namespace SmartIssueTrackerAPI.Models
{
    public class Comment
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Author { get; set; } = "System";
        public string Content { get; set; } = string.Empty;
    }
}
