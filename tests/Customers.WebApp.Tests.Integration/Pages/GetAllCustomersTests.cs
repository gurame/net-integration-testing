using Bogus;
using Customers.WebApp.Models;
using FluentAssertions;
using Microsoft.Playwright;

namespace Customers.WebApp.Tests.Integration.Pages;

[Collection("TestCollection")]
public class GetAllCustomersTests
{
	private readonly SharedTestContext _testContext;
	private readonly Faker<Customer> _customerFaker = 
	new Faker<Customer>()
		.RuleFor(x => x.Name, f => f.Person.FullName)
		.RuleFor(x => x.Email, f => f.Person.Email)
		.RuleFor(x => x.GitHubUsername, SharedTestContext.ValidGitHubUser)
		.RuleFor(x => x.DateOfBirth, f => DateOnly.FromDateTime(f.Person.DateOfBirth));
    public GetAllCustomersTests(SharedTestContext testContext)
    {
        _testContext = testContext;
    }

	[Fact]
	public async Task GetAll_ConstainsCustomer_WhenCustomerExists()
    {
        //Arrange
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions()
        {
            BaseURL = SharedTestContext.WebAppUrl
        });
        var customer = await CreateCustomer(page);
        var url = $"{SharedTestContext.WebAppUrl}/customers";
        
        //Act
        await page.GotoAsync(url);
        var name = page.Locator("tbody>tr>td").Filter(
            new LocatorFilterOptions { HasTextString = customer.Name });

		//Assert
        (await name.First.InnerTextAsync()).Should().Be(customer.Name);

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
        return customer;
    }
}
