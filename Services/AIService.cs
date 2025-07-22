// Services/AIService.cs
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartIssueTrackerAPI.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public AIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenAI:ApiKey"];
        }

        public async Task<string> SummarizeAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                return $"[Mocked AI Summary] {text.Substring(0, Math.Min(text.Length, 100))}...";
            }

            Console.WriteLine($"[AI] Using Key: {_apiKey.Substring(0, 10)}...");
            Console.WriteLine($"[AI] Sending request to OpenAI...");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant that summarizes issue tracker threads." },
            new { role = "user", content = text }
        }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);

            Console.WriteLine($"[AI] OpenAI response: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                return "[Rate Limit Reached] Too many requests to OpenAI. Try again later.";
            }

            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resultJson);
            var completion = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return completion ?? "(No summary returned)";
        }

    }
}