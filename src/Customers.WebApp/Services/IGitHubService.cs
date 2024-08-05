namespace Customers.WebApp.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
