namespace Customers.Api.Endpoints.Internal;
public interface IEndpoint
{
	public static abstract void DefineEnpoints(IEndpointRouteBuilder app);
	public static abstract void AddServices(IServiceCollection services); 
}
