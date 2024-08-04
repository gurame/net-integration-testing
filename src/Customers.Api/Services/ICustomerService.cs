using Customers.Api.Contracts;
using ErrorOr;
using FluentValidation;

namespace Customers.Api.Services;

public interface ICustomerService
{
	Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync();
	Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId);
	Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest request);
	Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid customerId, CustomerRequest request);
	Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid customerId);
}
