using Customers.Api.Contracts;
using ErrorOr;
using FluentValidation;

namespace Customers.Api.Services;

public interface ICustomerService
{
	Task<ErrorOr<IEnumerable<CustomerResponse>>> GetCustomersAsync(string? searchTerm);
	Task<ErrorOr<CustomerResponse>> GetCustomerAsync(Guid customerId);
	Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid customerId);
}

public interface ICustomerWriterService
{
	Task<ErrorOr<CustomerResponse>> CreateCustomerAsync(CustomerRequest request);
	Task<ErrorOr<CustomerResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request);
}
