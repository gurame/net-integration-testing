using Customers.WebApp.Data;
using Customers.WebApp.Models;

namespace Customers.WebApp.Mapping;

public static class ModelToDtoMapper
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            Name = customer.Name,
            DateOfBirth = customer.DateOfBirth.ToDateTime(TimeOnly.MinValue)
        };
    }
}
