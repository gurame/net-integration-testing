using Bogus;
using Customers.WebApp.Models;
using FluentAssertions;
using Microsoft.Playwright;

namespace Customers.WebApp.Tests.Integration.Pages;

[Collection("TestCollection")]
public class UpdateCustomerTests
{
	private readonly SharedTestContext _testContext;
	private readonly Faker<Customer> _customerFaker = 
	new Faker<Customer>()
		.RuleFor(x => x.Name, f => f.Person.FullName)
		.RuleFor(x => x.Email, f => f.Person.Email)
		.RuleFor(x => x.GitHubUsername, SharedTestContext.ValidGitHubUser)
		.RuleFor(x => x.DateOfBirth, f => DateOnly.FromDateTime(f.Person.DateOfBirth));
    public UpdateCustomerTests(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

	[Fact]
	public async Task Update_UpdatesCustomer_WhenDataIsValid()
	{
        //Arrange
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions()
        {
            BaseURL = SharedTestContext.WebAppUrl
        });
        var customer = await CreateCustomer(page);
		await page.GotoAsync($"update-customer/{customer.CustomerId}");
		customer.Name = "Another Name";

        //Act
        await page.FillAsync("input[id=fullname]", customer.Name);
		await page.ClickAsync("button[type=submit]");

		//Assert
		var linkElement = page.Locator("article>p>a").First;
		var link = await linkElement.GetAttributeAsync("href");
		await page.GotoAsync(link!);

        (await page.Locator("p[id=fullname-field]").InnerTextAsync()).Should().Be(customer.Name);
        (await page.Locator("p[id=email-field]").InnerTextAsync()).Should().Be(customer.Email);
        (await page.Locator("p[id=github-username-field]").InnerTextAsync()).Should().Be(customer.GitHubUsername);
        (await page.Locator("p[id=dob-field]").InnerTextAsync()).Should().Be(customer.DateOfBirth.ToString("dd/MM/yyyy"));

        await page.CloseAsync();
	}

	private async Task<Customer> CreateCustomer(IPage page)
    {
        var customer = _customerFaker.Generate();
        await page.GotoAsync("add-customer");
        await page.FillAsync("input[id=fullname]", customer.Name);
        await page.FillAsync("input[id=email]", customer.Email);
        await page.FillAsync("input[id=github-username]", customer.GitHubUsername);
        await page.FillAsync("input[id=dob]", customer.DateOfBirth.ToString("yyyy-MM-dd"));
        await page.ClickAsync("button[type=submit]");

		var element = page.Locator("article>p>a").First;
		var link = await element!.GetAttributeAsync("href");
		var idInText = link!.Split("/").Last();
		customer.CustomerId = Guid.Parse(idInText);
        return customer;
    }
}
