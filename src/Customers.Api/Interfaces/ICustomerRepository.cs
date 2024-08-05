using Customers.Api.Domain;

namespace Customers.Api.Interfaces;

public interface ICustomerRepository
{
	Task<IEnumerable<Customer>> GetAllAsync(string? searchTerm);
	Task<Customer> GetAsync(Guid customerId);
	Task<bool> CreateAsync(Customer customer);
	Task<bool> UpdateAsync(Customer customer);
	Task<bool> DeleteAsync(Customer customer);
}
