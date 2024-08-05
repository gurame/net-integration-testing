namespace Customers.Api.Domain;

public class Customer
{
	public Guid CustomerId { get; init; } = Guid.NewGuid();
	public string Name { get; init; } = default!;
	public string Email { get; init; } = default!;
	public string GitHubUserName { get; init; } = default!;
	public DateTime DateOfBirth { get; init; }
}
