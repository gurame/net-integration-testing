namespace Customers.Api.Services;

public interface IGitHubService
{
	Task<bool> UserExists(string userName);
}
