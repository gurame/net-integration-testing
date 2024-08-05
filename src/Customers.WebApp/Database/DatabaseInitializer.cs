﻿using System.Transactions;
using Dapper;

namespace Customers.WebApp.Database;

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
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Customers (
                CustomerId UUID PRIMARY KEY, 
                GitHubUsername TEXT NOT NULL,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                DateOfBirth TEXT NOT NULL)");
    }
}