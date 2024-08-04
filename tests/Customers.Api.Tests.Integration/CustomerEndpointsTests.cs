using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Customers.Api.Tests.Integration;

public class CustomerEndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client;
    public CustomerEndpointsTests(WebApplicationFactory<IApiMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExists()
    {
        // Act
        var response = await _client.GetAsync($"/customers/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Title.Should().Be("Resource not found");
        problem.Status.Should().Be((int)HttpStatusCode.NotFound);
    }
}