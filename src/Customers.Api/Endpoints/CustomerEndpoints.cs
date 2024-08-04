using Customers.Api.Contracts;
using Customers.Api.Endpoints.Internal;
using Customers.Api.Extensions;
using Customers.Api.Infrastructure.Data;
using Customers.Api.Interfaces;
using Customers.Api.Services;

namespace Customers.Api.Endpoints;

public class CustomerEndpoints : IEndpoint
{
	const string BasePath = "/customers";
	const string Tag = "Customers";
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
		services.AddScoped<ICustomerService, CustomerService>();
    }
    public static void DefineEnpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(BasePath, async (ICustomerService service) => {
			var result = await service.GetCustomersAsync();
			return result.ToMinimalApiResult();
		})
		.WithTags(Tag);

		app.MapGet($"{BasePath}/{{id:guid}}", async (ICustomerService service, Guid id) => {
			var result = await service.GetCustomerAsync(id);
			return result.ToMinimalApiResult();
		})
		.WithTags(Tag);

		app.MapPost($"{BasePath}", async (ICustomerService service, CustomerRequest request) => {
			var result = await service.CreateCustomerAsync(request);
			return result.ToMinimalApiResult();
		})
		.WithTags(Tag);

		app.MapPut($"{BasePath}/{{id:guid}}", async (ICustomerService service, Guid id, CustomerRequest request) => {
			var result = await service.UpdateCustomerAsync(id, request);
			return result.ToMinimalApiResult();
		})
		.WithTags(Tag);

		app.MapDelete($"{BasePath}/{{id:guid}}", async (ICustomerService service, Guid id) => {
			var result = await service.DeleteCustomerAsync(id);
			return result.ToMinimalApiResult();
		})
		.WithTags(Tag);
		
    }
}
