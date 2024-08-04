using Customers.Api.Contracts;
using ErrorOr;

namespace Customers.Api.Services;

public interface ICustomerService
{
	Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync();
	Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId);
	Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest customer);
	Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid customerId, CustomerRequest customer);
	Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid customerId);
}
