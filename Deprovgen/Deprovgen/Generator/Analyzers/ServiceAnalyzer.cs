using System.Linq;
using Deprovgen.Generator.Domains;
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
			var varName = _named.Name[0].ToString().ToLower() + _named.Name.Substring(1);

			var dependencies = _named.Constructors[0].Parameters
				.Select(x => x.Type)
				.OfType<INamedTypeSymbol>()
				.Select(x => new ServiceAnalyzer(x))
				.Select(x => x.GetServiceDefinition())
				.ToArray();

			return new ServiceDefinition(_named.GetFullNameSpace(),
				_named.Name,
				"_" + varName,
				varName,
				dependencies);
		}
	}
}
