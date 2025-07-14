namespace SmartIssueTrackerAPI.Services
{
    public class AIService
    {
        public async Task<string> SummarizeAsync(string content)
        {
            await Task.Delay(200); // simulate AI call
            return $"[AI Summary of {content.Length} chars]";
        }
    }
}
