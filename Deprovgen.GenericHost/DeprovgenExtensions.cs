using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Deprovgen.GenericHost
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
