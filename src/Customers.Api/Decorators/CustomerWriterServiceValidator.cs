using Customers.Api.Contracts;
using Customers.Api.Extensions;
using Customers.Api.Services;
using ErrorOr;
using FluentValidation;

namespace Customers.Api.Decorators;

public class CustomerWriterServiceValidator : ICustomerWriterService
{
	private readonly ICustomerWriterService _service;
	private readonly IValidator<CustomerRequest> _validator;
    public CustomerWriterServiceValidator(ICustomerWriterService service, IValidator<CustomerRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    public async Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
		if (!validationResult.IsValid)
			return validationResult.ToErrorOr<CustomerResponse>(null!);
			
		return await _service.CreateCustomerAsync(request);
    }

    public async Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
		if (!validationResult.IsValid)
			return validationResult.ToErrorOr<CustomerResponse>(null!);
			
		return await _service.UpdateCustomerAsync(id, request);
    }
}
