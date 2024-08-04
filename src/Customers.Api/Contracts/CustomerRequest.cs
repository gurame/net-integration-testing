namespace Customers.Api.Contracts;

public record CustomerRequest(string Name, string Email, string GitHubUserName, DateTime DateOfBirth);