using System;
using System.Linq;
using Deprovgen.Annotations;
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
			Logger.WriteLine($"Resolver: {_methodSymbol.ReturnType} {_methodSymbol.Name}").Wait();

			var serviceType = GetServiceType();
			var service = new ServiceAnalyzer(serviceType);
			var parameters = GetParameters();
			bool isTransient = _methodSymbol.Name.EndsWith("AsTransient");
			return new ResolverDefinition(_methodSymbol.Name,
				service.GetServiceDefinition(),
				parameters,
				isTransient,
				serviceType.DeclaredAccessibility,
				_methodSymbol.ReturnType);
		}

		private INamedTypeSymbol GetServiceType()
		{
			var attr = _methodSymbol.GetAttributes()
				.FirstOrDefault(x => x.AttributeClass.Name == nameof(ImplementationAttribute));

			if (attr?.ConstructorArguments[0].Value is INamedTypeSymbol type)
			{
				return type;
			}
			else if (_methodSymbol.ReturnType is INamedTypeSymbol named)
			{
				return named;
			}
			throw new Exception();
		}

		private VariableDefinition[] GetParameters()
		{
			return _methodSymbol.Parameters
				.Select(x => new VariableDefinition(x.Type.Name, x.Name, x.Type.GetFullNameSpace()))
				.ToArray();
		}

		public static ResolverDefinition[] GetResolverDefinitions(INamedTypeSymbol factorySymbol)
		{
			// ミックスインのために基底インターフェースのメソッドを取得する
			var baseInterfaces = factorySymbol.AllInterfaces
				.SelectMany(x => x.GetMembers())
				.OfType<IMethodSymbol>();

			return factorySymbol.GetMembers()
				.OfType<IMethodSymbol>()
				.Concat(baseInterfaces)
				.Where(x => x.MethodKind == MethodKind.Ordinary)
				.Select(x => new ResolverAnalyzer(x))
				.Select(x => x.GetResolverDefinition())
				.ToArray();
		}
	}
}
