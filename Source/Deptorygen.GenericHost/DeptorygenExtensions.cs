using Microsoft.Extensions.DependencyInjection;

namespace Deptorygen.GenericHost
{
	public static class DeptorygenExtensions
	{
		public static IServiceCollection UseDeprovgenFactory(this IServiceCollection self, IDeptorygenFactory factory)
		{
			factory.ConfigureServices(self);
			return self;
		}
	}
}
