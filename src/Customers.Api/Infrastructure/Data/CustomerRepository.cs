using Customers.Api.Domain;
using Customers.Api.Interfaces;
using Dapper;

namespace Customers.Api.Infrastructure.Data;
public class CustomerRepository : ICustomerRepository
{
	private readonly IDbConnectionFactory _dbConnectionFactory;
    public CustomerRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
	public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        string sql = "SELECT * FROM Customers";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.QueryAsync<Customer>(sql);
    }
	public async Task<Customer> GetCustomerAsync(Guid customerId)
    {
        string sql = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return (await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { CustomerId = customerId }))!;
    }
    public async Task<bool> CreateCustomerAsync(Customer customer)
    {
        string sql = @"INSERT INTO Customers (CustomerId, Name, Email, GitHubUserName, DateOfBirth) 
						VALUES (@CustomerId, @Name, @Email, @GitHubUserName, @DateOfBirth)";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, customer) > 0;
    }
    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        string sql = @"UPDATE Customers
						SET Name = @Name, Email = @Email, GitHubUserName = @GitHubUserName, DateOfBirth = @DateOfBirth
						WHERE CustomerId = @CustomerId";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, customer) > 0;
    }
	public async Task<bool> DeleteCustomerAsync(Guid customerId)
    {
        string sql = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, new { CustomerId = customerId }) > 0;
    }
}
