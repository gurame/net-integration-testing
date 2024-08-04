using Customers.Api.Domain;

namespace Customers.Api.Interfaces;

public interface ICustomerRepository
{
	Task<IEnumerable<Customer>> GetCustomersAsync();
	Task<Customer> GetCustomerAsync(Guid customerId);
	Task<bool> CreateCustomerAsync(Customer customer);
	Task<bool> UpdateCustomerAsync(Customer customer);
	Task<bool> DeleteCustomerAsync(Guid customerId);
}
