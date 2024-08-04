namespace Customers.Api.Domain;

public class Customer
{
	public Guid CustomerId { get; set; }
	public string Name { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string GitHubUserName { get; set; } = default!;
	public DateTime DateOfBirth { get; set; }
}
