namespace Customers.Api.Contracts;

public record CustomerResponse(Guid CustomerId, string Name, string Email, string GitHubUserName, DateTime DateOfBirth);
