using Customers.Api.Contracts;
using Customers.Api.Domain;
using Customers.Api.Interfaces;
using ErrorOr;
using Mapster;

namespace Customers.Api.Services;
public class CustomerService : ICustomerService
{
	private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
	public async Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
		if (customer is null)
			return Error.NotFound($"Customer with id {customerId} not found");
		
		return customer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync()
    {
        var customers = await _customerRepository.GetCustomersAsync();
		return customers.Select(c => c.Adapt<CustomerResponse>()).ToList();
    }
    public async Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest customer)
    {
		var newCustomer = customer.Adapt<Customer>();
		newCustomer.CustomerId = Guid.NewGuid();
		var result = await _customerRepository.CreateCustomerAsync(newCustomer);
		if (!result)
			return Error.Failure("Failed to create customer");

		var createdCustomer = await _customerRepository.GetCustomerAsync(newCustomer.CustomerId);
		return createdCustomer.Adapt<CustomerResponse>();
    }
    public async Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid customerId, CustomerRequest request)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
		if (customer is null)
			return Error.NotFound($"Customer with id {customerId} not found");

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
			return Error.NotFound($"Customer with id {customerId} not found");

		var result = await _customerRepository.DeleteCustomerAsync(customerId);
		if (!result)
			return Error.Failure("Failed to delete customer");

		return Result.Deleted;
    }
}
