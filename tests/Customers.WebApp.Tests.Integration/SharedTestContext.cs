
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Builders;
using Microsoft.Playwright;

namespace Customers.WebApp.Tests.Integration;

public class SharedTestContext : IAsyncLifetime
{
    public const string ValidGitHubUser = "valid-user";
    public const string WebAppUrl = "http://localhost:7779";
    public GitHubApiServer GitHubApiServer { get; } = new();
    
    private static readonly string DockerComposeFile = Path.Combine(
        Directory.GetCurrentDirectory(),
        (TemplateString)"../../../docker-compose.integration.yml");

    private readonly ICompositeService _dockerCompose = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFile)
        .RemoveOrphans()
        .WaitForHttp("test-app", WebAppUrl)
        .Build();

    private IPlaywright _playwright;
    public IBrowser Browser { get; private set; }
        
    public async Task InitializeAsync()
    {
        GitHubApiServer.Start();
        GitHubApiServer.SetUpUser(ValidGitHubUser);
        _dockerCompose.Start();

        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000
        });
    }
    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright.Dispose();
    
        _dockerCompose.Dispose();
        GitHubApiServer.Dispose();
    }
}