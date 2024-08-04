using System.Data;

namespace Customers.Api.Infrastructure.Data;

public interface IDbConnectionFactory
{
	Task<IDbConnection> CreateConnectionAsync();
}
