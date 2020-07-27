using Microsoft.Extensions.DependencyInjection;

namespace Deptorygen.GenericHost
{
	public interface IDeprovgenFactory
	{
		void ConfigureServices(IServiceCollection services);
	}
}
