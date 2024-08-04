using Customers.Api.Contracts;
using FluentValidation;

namespace Customers.Api.Validators;
public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
	public CustomerRequestValidator()
	{
		RuleFor(x=> x.Name).NotEmpty();
		RuleFor(x=> x.Email).NotEmpty().EmailAddress();
		RuleFor(x=> x.GitHubUserName).NotEmpty();
		RuleFor(x=> x.DateOfBirth).NotEmpty().LessThan(DateTime.Now);
	}
}
