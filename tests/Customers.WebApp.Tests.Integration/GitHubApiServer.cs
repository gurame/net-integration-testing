using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customers.WebApp.Tests.Integration;

public class GitHubApiServer : IDisposable
{
    private const int Port = 9850;
    private WireMockServer _server = null!;

    public string Url => _server.Url!;

    public void Start()
	{
		_server = WireMockServer.Start(Port);
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
		GC.SuppressFinalize(this);
    }
}
