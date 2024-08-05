using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Contracts;

public class UpdateCustomerRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public CustomerRequest Customer { get; set; } = default!;
}
