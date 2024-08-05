using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;

namespace Customers.Api.Tests.Integration.CustomerEndpoints;

public class GetAllCustomerEndpointTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient _client;
	private readonly Faker<CustomerRequest> _customerFaker = 
	new Faker<CustomerRequest>()
		.RuleFor(x => x.Name, f => f.Person.FullName)
		.RuleFor(x => x.Email, f => f.Person.Email)
		.RuleFor(x => x.GitHubUserName, CustomerApiFactory.GitHubUser)
		.RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date);
	public GetAllCustomerEndpointTests(CustomerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

	[Fact]
	public async Task Get_ReturnsCustomers_WhenCustomersExists()
	{
		// Arrange
		var customerRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", customerRequest);
		var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

		// Act
		var response = await _client.GetAsync($"/customers?q={customerRequest.Email}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var customers = await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
		customers.Should().NotBeEmpty();
		customers.Should().ContainEquivalentOf(customerResponse);
	}

	[Fact]
	public async Task Get_ReturnsEmptyList_WhenCustomersDontExists()
	{
		// Act
		var response = await _client.GetAsync($"/customers?q={Guid.NewGuid()}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var customers = await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
		customers.Should().BeEmpty();
	}
}
