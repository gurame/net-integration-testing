using Customers.Api.Contracts;
using Customers.Api.Domain;
using Customers.Api.Interfaces;
using ErrorOr;
using Mapster;

namespace Customers.Api.Services;
public class CustomerService : ICustomerService, ICustomerWriterService
{
	private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    public CustomerService(ICustomerRepository customerRepository, 
			IGitHubService gitHubService)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
    }
    public async Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {customerId} not found");
		
		return customer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync(string? searchTerm)
    {
        var customers = await _customerRepository.GetAllAsync(searchTerm);
		return customers.Select(c => c.Adapt<CustomerResponse>()).ToList();
    }
    public async Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest request)
    {
		var userExists = await _gitHubService.UserExists(request.GitHubUserName);
		if (!userExists)
			return Error.Validation("GitHubUserName", $"GitHub user {request.GitHubUserName} not found");

		var customer = request.Adapt<Customer>();
		var result = await _customerRepository.CreateAsync(customer);
		if (!result)
			return Error.Failure(description: "Failed to create customer");

		return customer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request)
    {
        var customer = await _customerRepository.GetAsync(id);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {id} not found");

		var userExists = await _gitHubService.UserExists(request.GitHubUserName);
		if (!userExists)
			return Error.Validation("GitHubUserName", $"GitHub user {request.GitHubUserName} not found");

		request.Adapt(customer);
		var result = await _customerRepository.UpdateAsync(customer);
		if (!result)
			return Error.Failure("Failed to update customer");

		return customer.Adapt<CustomerResponse>();
    }
	public async Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);
		if (customer is null)
			return Error.NotFound(description: $"Customer with id {customerId} not found");

		var result = await _customerRepository.DeleteAsync(customer);
		if (!result)
			return Error.Failure(description: "Failed to delete customer");

		return Result.Deleted;
    }
}
