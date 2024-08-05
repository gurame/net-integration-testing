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
	public async Task<IEnumerable<Customer>> GetAllAsync(string? searchTerm)
    {
        string sql = "SELECT * FROM Customers WHERE @SearchTerm Is Null Or (Name LIKE '%' || @SearchTerm || '%' OR Email LIKE '%' || @SearchTerm || '%')";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.QueryAsync<Customer>(sql, new { SearchTerm = searchTerm });
    }
	public async Task<Customer> GetAsync(Guid customerId)
    {
        string sql = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return (await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { CustomerId = customerId }))!;
    }
    public async Task<bool> CreateAsync(Customer customer)
    {
        string sql = @"INSERT INTO Customers (CustomerId, Name, Email, GitHubUserName, DateOfBirth) 
                        VALUES (@CustomerId, @Name, @Email, @GitHubUserName, @DateOfBirth)";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, customer) > 0;
    }
    public async Task<bool> UpdateAsync(Customer customer)
    {
        string sql = @"UPDATE Customers
						SET Name = @Name, Email = @Email, GitHubUserName = @GitHubUserName, DateOfBirth = @DateOfBirth
						WHERE CustomerId = @CustomerId";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, customer) > 0;
    }
	public async Task<bool> DeleteAsync(Customer customer)
    {
        string sql = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.ExecuteAsync(sql, new { customer.CustomerId }) > 0;
    }
}
