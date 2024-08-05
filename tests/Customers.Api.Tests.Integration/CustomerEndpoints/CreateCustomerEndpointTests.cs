using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integration.CustomerEndpoints;
public class CreateCustomerEndpointTests : IClassFixture<CustomerApiFactory>
{
	private readonly HttpClient _client;
    private readonly Faker<CustomerRequest> _customerFaker = 
        new Faker<CustomerRequest>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.GitHubUserName, CustomerApiFactory.GitHubUser)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date);
    public CreateCustomerEndpointTests(CustomerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

	[Fact]
    public async Task Create_ReturnsOk_WhenCustomerIsCreated()
    {
        // Arrange
        var customerRequest = _customerFaker.Generate();

        // Act
        var createdResponse = await _client.PostAsJsonAsync($"/customers", customerRequest);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        createdCustomer.Should().BeEquivalentTo(customerRequest, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenEmailIsNotValid()
    {
        // Arrange
        var customerRequest = _customerFaker.Clone()
            .RuleFor(x => x.Email, f => f.Random.String2(10))
            .Generate();

        // Act
        var response = await _client.PostAsJsonAsync($"/customers", customerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails!.Errors["Email"].Should().Contain("'Email' is not a valid email address.");
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenGitHubUserDoesNotExists()
    {
        // Arrange
        var customerRequest = _customerFaker.Clone()
            .RuleFor(x => x.GitHubUserName, f => f.Random.String2(10))
            .Generate();

        // Act
        var response = await _client.PostAsJsonAsync($"/customers", customerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails!.Errors["GitHubUserName"].Should().Contain($"GitHub user {customerRequest.GitHubUserName} not found");
    }
}
