using System.Reflection;

namespace Customers.Api.Endpoints.Internal;

public static class EndpointExtensions
{
	public static void AddEndpoints<TMarker>(this IServiceCollection services)
	{
		AddEndpoints(services, typeof(TMarker));
	}
	public static void AddEndpoints(this IServiceCollection services, Type typeMarker)
	{
		var endpoints = GetEndpointTypesFromAssemblyContaining(typeMarker);
		foreach (var endpoint in endpoints)
		{
			endpoint.GetMethod(nameof(IEndpoint.AddServices))?.Invoke(null, [services]);
		}
	}
	public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
	{
		UseEndpoints(app, typeof(TMarker));
	}
	public static void UseEndpoints(this IApplicationBuilder app, Type typeMarker)
	{
		var endpoints = GetEndpointTypesFromAssemblyContaining(typeMarker);
		foreach (var endpoint in endpoints)
		{
			endpoint.GetMethod(nameof(IEndpoint.DefineEnpoints))?.Invoke(null, [app]);
		}
	}
	private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
	{
		var endpoints = typeMarker.Assembly.DefinedTypes
			.Where(x=> !x.IsAbstract && !x.IsInterface && typeof(IEndpoint).IsAssignableFrom(x));
		return endpoints;
	}
}
