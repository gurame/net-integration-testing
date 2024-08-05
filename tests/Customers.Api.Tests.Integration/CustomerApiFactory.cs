using Customers.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace Customers.Api.Tests.Integration;

public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
	public const string GitHubUser = "gurame";
	private readonly PostgreSqlContainer _dbContainer = 
		new PostgreSqlBuilder()
		.WithDatabase("testing")
		.WithUsername("postgres")
		.WithPassword("postgres")
		.Build();
	private readonly GitHubApiServer _gitHubApiServer = new ();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
		{
			logging.ClearProviders();
		});

		builder.ConfigureTestServices(services =>
		{
			services.RemoveAll(typeof(IDbConnectionFactory));
			services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(_dbContainer.GetConnectionString()));

			services.AddHttpClient("GitHub", client=> 
			{
				client.BaseAddress = new Uri(_gitHubApiServer.Url);
			});
		});
    }

	public async Task InitializeAsync() 
	{
		_gitHubApiServer.Start();
		_gitHubApiServer.SetUpUser(GitHubUser);
		await _dbContainer.StartAsync();
	}

    public new async Task DisposeAsync()
	{
		await _dbContainer.DisposeAsync();
		_gitHubApiServer.Dispose();	
	}
}
