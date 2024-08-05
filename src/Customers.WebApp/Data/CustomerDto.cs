namespace Customers.WebApp.Data;

public class CustomerDto
{
    public Guid CustomerId { get; init; } = default!;

    public string GitHubUsername { get; init; } = default!;

    public string Name { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateTime DateOfBirth { get; init; }
}
