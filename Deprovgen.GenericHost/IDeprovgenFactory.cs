using System;
using Microsoft.Extensions.DependencyInjection;

namespace Deprovgen.GenericHost
{
	public interface IDeprovgenFactory
	{
		void ConfigureServices(IServiceCollection services);
	}
}
