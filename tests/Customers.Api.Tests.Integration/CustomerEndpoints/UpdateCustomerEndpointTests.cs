using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integration.CustomerEndpoints;
public class UpdateCustomerEndpointTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient _client;
    private readonly Faker<CustomerRequest> _customerFaker = 
        new Faker<CustomerRequest>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.GitHubUserName, CustomerApiFactory.GitHubUser)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date);
    public UpdateCustomerEndpointTests(CustomerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

	[Fact]
    public async Task Update_ReturnsOk_WhenCustomerIsUpdated()
    {
        // Arrange
        var createRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", createRequest);
		var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
		var updateRequest = _customerFaker.Clone().Generate();

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/customers/{createdCustomer!.CustomerId}", updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedCustomer = await updateResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        updatedCustomer.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCustomerDoesNotExists()
    {
        // Arrange
        var updateRequest = _customerFaker.Generate();

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/customers/{Guid.NewGuid()}", updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await updateResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Title.Should().Be("Resource not found");
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var createRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", createRequest);
		var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
		var updateRequest = _customerFaker.Clone()
							.RuleFor(x => x.Email, f => "invalid-email")
							.Generate();

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/customers/{createdCustomer!.CustomerId}", updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails!.Errors["Email"].Should().Contain("'Email' is not a valid email address.");
    }

	[Fact]
    public async Task Update_ReturnsBadRequest_WhenGitHubUserDoesNotExists()
    {
        // Arrange
        var createRequest = _customerFaker.Generate();
		var createResponse = await _client.PostAsJsonAsync($"/customers", createRequest);
		var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
		var updateRequest = _customerFaker.Clone()
							.RuleFor(x => x.GitHubUserName, f => "invalid-githubuser")
							.Generate();

        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/customers/{createdCustomer!.CustomerId}", updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails!.Errors["GitHubUserName"].Should().Contain($"GitHub user {updateRequest.GitHubUserName} not found");
    }
}
