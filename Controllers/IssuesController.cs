using Microsoft.AspNetCore.Mvc;
using SmartIssueTrackerAPI.Models;
using SmartIssueTrackerAPI.Services;

namespace SmartIssueTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IssueStore _store;
        private readonly AIService _ai;

        public IssuesController(IssueStore store, AIService ai)
        {
            _store = store;
            _ai = ai;
        }

        [HttpGet]
        public ActionResult<List<Issue>> GetAll() => _store.GetAll();

        [HttpPost]
        public IActionResult Create(Issue issue)
        {
            _store.Add(issue);
            return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var issue = _store.Get(id);
            return issue is null ? NotFound() : Ok(issue);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Issue updated)
        {
            var existing = _store.Get(id);
            if (existing is null) return NotFound();

            updated.Id = id;
            _store.Update(updated);
            return NoContent();
        }

        [HttpPost("{id}/comment")]
        public IActionResult AddComment(Guid id, Comment comment)
        {
            var issue = _store.Get(id);
            if (issue is null) return NotFound();

            issue.Comments.Add(comment);
            _store.Update(issue);
            return Ok(issue);
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> Summarize(Guid id)
        {
            var issue = _store.Get(id);
            if (issue is null) return NotFound();

            var content = issue.Description + string.Join("\n", issue.Comments.Select(c => c.Content));
            var summary = await _ai.SummarizeAsync(content);
            return Ok(new { summary });
        }
    }
}
