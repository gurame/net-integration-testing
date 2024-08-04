using Dapper;

namespace Customers.Api.Infrastructure.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;
    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Customers (
				CustomerId UUID PRIMARY KEY,
				Name TEXT NOT NULL,
				Email TEXT NOT NULL,
				GitHubUserName TEXT NOT NULL,
				DateOfBirth DATE NOT NULL)"
            );
    }
}
