using System;
using System.Collections.Generic;
using System.Linq;
using Deprovgen.Annotations;
using Deprovgen.Generator.Domains;
using Deprovgen.Utilities;
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

		public CollectionResolverDefinition GetCollectionResolverDefinition()
		{
			var elementType = GetElementType();
			if (elementType == null) return null;

			var serviceTypes = _methodSymbol.GetAttributes()
				.SelectMany(x => IsMatchAsCollectionResolver(elementType, x))
				.ToArray();

			if (serviceTypes.Any())
			{
				var services = serviceTypes.Select(x => new ServiceAnalyzer(x))
					.Select(x => x.GetServiceDefinition())
					.ToArray();
				var accessibilities = serviceTypes.Select(x => x.DeclaredAccessibility)
					.ToArray();

				return new CollectionResolverDefinition(
					elementType,
					_methodSymbol.Name,
					services,
					GetParameters(),
					GetMostStrictAccessibility(accessibilities));
			}

			return null;
		}

		private TypeName GetElementType()
		{
			TypeName elementType;
			if (_methodSymbol.ReturnType is INamedTypeSymbol named
			    && named.IsGenericType)
			{
				if (named.Name == "IEnumerable"
				    && named.TypeArguments[0] is INamedTypeSymbol et)
				{
					elementType = TypeName.FromSymbol(et);
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}

			return elementType;
		}

		private Accessibility GetMostStrictAccessibility(Accessibility[] accessibilities)
		{
			return accessibilities.All(x => x == Accessibility.Public) ? Accessibility.Public : Accessibility.Internal;
		}

		private IEnumerable<INamedTypeSymbol> IsMatchAsCollectionResolver(TypeName elementType, AttributeData attribute)
		{
			if (attribute.AttributeClass.Name != nameof(ResolutionAttribute))
			{
				yield break;
			}

			if (attribute.ConstructorArguments[0].Value is INamedTypeSymbol type)
			{
				if (TypeName.FromSymbol(type.BaseType) == elementType)
				{
					yield return type;
					yield break;
				}

				foreach (var @interface in type.AllInterfaces)
				{
					if (TypeName.FromSymbol(@interface) == elementType)
					{
						yield return @interface;
						yield break;
					}
				}
			}
		}

		private INamedTypeSymbol GetServiceType()
		{
			var attr = _methodSymbol.GetAttributes()
				.FirstOrDefault(x => x.AttributeClass.Name == nameof(ResolutionAttribute));

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
				.Select(x => new VariableDefinition(TypeName.FromSymbol(x.Type), x.Name))
				.ToArray();
		}

		public static ResolverDefinition[] GetResolverDefinitions(INamedTypeSymbol factorySymbol)
		{
			return GetAnalyzers(factorySymbol)
				.Select(x => x.GetResolverDefinition())
				.ToArray();
		}

		public static CollectionResolverDefinition[] GetCollectionResolverDefinitions(INamedTypeSymbol factorySymbol)
		{
			return GetAnalyzers(factorySymbol)
				.Select(x => x.GetCollectionResolverDefinition())
				.Where(x => x != null)
				.ToArray();
		}

		private static ResolverAnalyzer[] GetAnalyzers(INamedTypeSymbol factorySymbol)
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
				.ToArray();
		}
	}
}
