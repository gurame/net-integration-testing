using System.Net;

namespace Customers.Api.Services;

public class GitHubService : IGitHubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public GitHubService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<bool> UserExists(string userName)
    {
        var client = _httpClientFactory.CreateClient("GitHub");
		var response = await client.GetAsync($"/users/{userName}");
		return response.IsSuccessStatusCode;
    }
}