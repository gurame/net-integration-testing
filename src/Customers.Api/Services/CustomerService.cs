using Customers.Api.Contracts;
using Customers.Api.Domain;
using Customers.Api.Extensions;
using Customers.Api.Interfaces;
using ErrorOr;
using FluentValidation;
using Mapster;

namespace Customers.Api.Services;
public class CustomerService : ICustomerService
{
	private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    private readonly IValidator<CustomerRequest> _validator;

    public CustomerService(ICustomerRepository customerRepository, 
		IGitHubService gitHubService,
		IValidator<CustomerRequest> validator)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        _validator = validator;
    }
    public async Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {customerId} not found");
		
		return customer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync(string? searchTerm)
    {
        var customers = await _customerRepository.GetCustomersAsync(searchTerm);
		return customers.Select(c => c.Adapt<CustomerResponse>()).ToList();
    }
    public async Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest request)
    {
		var validationResult = await _validator.ValidateAsync(request);
		if (!validationResult.IsValid)
			return validationResult.ToErrorOr<CustomerResponse>(null!);

		var userExists = await _gitHubService.UserExists(request.GitHubUserName);
		if (!userExists)
			return Error.Validation("GitHubUserName", $"GitHub user {request.GitHubUserName} not found");

		var customer = request.Adapt<Customer>();
		customer.CustomerId = Guid.NewGuid();
		var result = await _customerRepository.CreateCustomerAsync(customer);
		if (!result)
			return Error.Failure(description: "Failed to create customer");

		var createdCustomer = await _customerRepository.GetCustomerAsync(customer.CustomerId);
		return createdCustomer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid customerId, CustomerRequest request)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {customerId} not found");

		var validationResult = await _validator.ValidateAsync(request);
		if (!validationResult.IsValid)
			return validationResult.ToErrorOr<CustomerResponse>(null!);

		var userExists = await _gitHubService.UserExists(request.GitHubUserName);
		if (!userExists)
			return Error.Validation("GitHubUserName", $"GitHub user {request.GitHubUserName} not found");

		request.Adapt(customer);
		var result = await _customerRepository.UpdateCustomerAsync(customer);
		if (!result)
			return Error.Failure("Failed to update customer");

		var updatedCustomer = await _customerRepository.GetCustomerAsync(customerId);
		return updatedCustomer.Adapt<CustomerResponse>();
    }
	public async Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {customerId} not found");

		var result = await _customerRepository.DeleteCustomerAsync(customerId);
		if (!result)
			return Error.Failure(description: "Failed to delete customer");

		return Result.Deleted;
    }
}
