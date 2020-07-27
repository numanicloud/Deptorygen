using Microsoft.Extensions.DependencyInjection;

namespace Deptorygen.GenericHost
{
	public static class DeprovgenExtensions
	{
		public static IServiceCollection UseDeprovgenFactory(this IServiceCollection self, IDeprovgenFactory factory)
		{
			factory.ConfigureServices(self);
			return self;
		}
	}
}
