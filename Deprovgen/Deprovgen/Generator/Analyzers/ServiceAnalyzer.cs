using System.Linq;
using Deprovgen.Generator.Domains;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Analyzers
{
	class ServiceAnalyzer
	{
		private readonly INamedTypeSymbol _named;

		public ServiceAnalyzer(INamedTypeSymbol typeSymbol)
		{
			_named = typeSymbol;
		}

		public ServiceDefinition GetServiceDefinition()
		{
			ServiceDefinition[] dependencies;

			if (_named.Constructors.Any())
			{
				dependencies = _named.Constructors[0].Parameters
					.Select(x => x.Type)
					.OfType<INamedTypeSymbol>()
					.Select(x => new ServiceAnalyzer(x))
					.Select(x => x.GetServiceDefinition())
					.ToArray();
			}
			else
			{
				dependencies = new ServiceDefinition[0];
			}

			return new ServiceDefinition(TypeName.FromSymbol(_named), dependencies);
		}
	}
}
