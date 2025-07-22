using SmartIssueTrackerAPI.Models;

namespace SmartIssueTrackerAPI.Services
{
    public class IssueStore
    {
        private readonly List<Issue> _issues = new();

        public List<Issue> GetAll() => _issues;

        public Issue? Get(Guid id) => _issues.FirstOrDefault(i => i.Id == id);

        public void Add(Issue issue) => _issues.Add(issue);

        public void Update(Issue updated)
        {
            var index = _issues.FindIndex(i => i.Id == updated.Id);
            if (index != -1) _issues[index] = updated;
        }

        public void Close(Guid id)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue != null)
                issue.Status = "Closed";
        }

    }
}
