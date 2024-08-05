using Customers.WebApp.Data;
using Customers.WebApp.Models;

namespace Customers.WebApp.Mapping;

public static class DtoToModelMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            CustomerId = customerDto.CustomerId,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            Name = customerDto.Name,
            DateOfBirth = DateOnly.FromDateTime(customerDto.DateOfBirth)
        };
    }
}
