using Microsoft.Extensions.DependencyInjection;

namespace Deptorygen.GenericHost
{
	public static class DeptorygenExtensions
	{
		public static IServiceCollection UseDeptorygenFactory(this IServiceCollection self, IDeptorygenFactory factory)
		{
			factory.ConfigureServices(self);
			return self;
		}
	}
}
