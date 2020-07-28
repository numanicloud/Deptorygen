using Microsoft.Extensions.DependencyInjection;

namespace Deptorygen.GenericHost
{
	public interface IDeptorygenFactory
	{
		void ConfigureServices(IServiceCollection services);
	}
}
