﻿using Customers.WebApp.Data;
using Customers.WebApp.Database;
using Dapper;

namespace Customers.WebApp.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CustomerRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (CustomerId, GitHubUsername, Name, Email, DateOfBirth) 
            VALUES (@CustomerId, @GitHubUsername, @Name, @Email, @DateOfBirth)",
            customer);
        return result > 0;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<CustomerDto>(
            "SELECT * FROM Customers WHERE CustomerId = @Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<CustomerDto>("SELECT * FROM Customers");
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Customers SET GitHubUsername = @GitHubUsername, Name = @Name, Email = @Email, 
                 DateOfBirth = @DateOfBirth WHERE CustomerId = @CustomerId",
            customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE CustomerId = @Id",
            new {Id = id});
        return result > 0;
    }
}