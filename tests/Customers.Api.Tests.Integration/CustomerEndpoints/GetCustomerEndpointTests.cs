using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integration.CustomerEndpoints;

public class GetCustomerEndpointTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient _client;
	private readonly Faker<CustomerRequest> _customerFaker = 
        new Faker<CustomerRequest>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.GitHubUserName, CustomerApiFactory.GitHubUser)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date);
			
	public GetCustomerEndpointTests(CustomerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

	[Fact]
	public async Task Get_ReturnsOk_WhenCustomerExists()
	{
		// Arrange
		var customerRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", customerRequest);
		var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

		// Act
		var response = await _client.GetAsync($"/customers/{customerResponse!.CustomerId}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var customer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
		customer.Should().BeEquivalentTo(customerResponse);
	}

	[Fact]
	public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExists()
	{
		// Act
		var response = await _client.GetAsync($"/customers/{Guid.NewGuid()}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
		problemDetails!.Title.Should().Be("Resource not found");
	}
}
