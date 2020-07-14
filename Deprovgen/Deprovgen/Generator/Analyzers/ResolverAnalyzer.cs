using System;
using System.Linq;
using Deprovgen.Generator.Domains;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Analyzers
{
	class ResolverAnalyzer
	{
		private readonly IMethodSymbol _methodSymbol;

		private ResolverAnalyzer(IMethodSymbol methodSymbol)
		{
			_methodSymbol = methodSymbol;
		}

		public ResolverDefinition GetResolverDefinition()
		{
			if (_methodSymbol.ReturnType is INamedTypeSymbol named)
			{
				var service = new ServiceAnalyzer(named);
				var parameters = GetParameters();
				bool isTransient = _methodSymbol.Name.EndsWith("AsTransient");
				return new ResolverDefinition(_methodSymbol.Name,
					service.GetServiceDefinition(),
					parameters,
					isTransient,
					named.DeclaredAccessibility);
			}
			throw new Exception();
		}

		private VariableDefinition[] GetParameters()
		{
			return _methodSymbol.Parameters
				.Select(x => new VariableDefinition(x.Type.Name, x.Name, x.Type.ContainingNamespace.Name))
				.ToArray();
		}

		public static ResolverDefinition[] GetResolverDefinitions(INamedTypeSymbol factorySymbol)
		{
			return factorySymbol.GetMembers()
				.OfType<IMethodSymbol>()
				.Select(x => new ResolverAnalyzer(x))
				.Select(x => x.GetResolverDefinition())
				.ToArray();
		}
	}
}
