namespace Customers.Api.Contracts;

/*
	In order tu use Bogus generator, we should define the record as conventional, not as primary constructor.
	Bogus require a parameterless constructor to work properly.
*/
public record CustomerRequest
{
	public string Name { get; init; } = default!;
	public string Email { get; init; }= default!;
	public string GitHubUserName { get; init; } = default!;
	public DateTime DateOfBirth { get; init; }
}