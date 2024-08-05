using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integration.CustomerEndpoints;
public class DeleteCustomerEndpointTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient _client;
    private readonly Faker<CustomerRequest> _customerFaker = 
        new Faker<CustomerRequest>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.GitHubUserName, CustomerApiFactory.GitHubUser)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date);
    public DeleteCustomerEndpointTests(CustomerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

	[Fact]
    public async Task Delete_ReturnsNoContent_WhenCustomerIsDeleted()
    {
        // Arrange
        var createRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", createRequest);
		var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/customers/{createdCustomer!.CustomerId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExists()
    {
        // Act
        var updateResponse = await _client.DeleteAsync($"/customers/{Guid.NewGuid()}");

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await updateResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Title.Should().Be("Resource not found");
    }
}
