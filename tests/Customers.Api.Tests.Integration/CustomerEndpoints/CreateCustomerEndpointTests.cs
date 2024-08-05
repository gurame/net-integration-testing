using System.Net;
using System.Net.Http.Json;
using Bogus;
using Customers.Api.Contracts;
using FluentAssertions;

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
        var response = await _client.PostAsJsonAsync($"/customers", customerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customerRequest, options => options.ExcludingMissingMembers());
    }
}
