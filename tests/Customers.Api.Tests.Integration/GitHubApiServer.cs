using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customers.Api.Tests.Integration;

public class GitHubApiServer : IDisposable
{
	private WireMockServer _server;

    public string Url => _server.Url!;

    public void Start()
	{
		_server = WireMockServer.Start();
	}

	public void SetUpUser(string userName)
	{
		_server.Given(
				Request.Create()
				.WithPath($"/users/{userName}"))
				.RespondWith(
					Response.Create()
					.WithStatusCode(200));
	}

	public void Dispose()
    {
        _server.Stop();
		_server.Dispose();
    }
}
