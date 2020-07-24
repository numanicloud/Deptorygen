using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Annotations;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Syntaxes
{
	class CollectionResolverSyntax
	{
		public string MethodName { get; }
		public TypeName ElementType { get; }
		public ParameterSyntax[] Parameters { get; }
		public ResolutionSyntax[] Resolutions { get; }

		public CollectionResolverSyntax(string methodName, TypeName elementType, ParameterSyntax[] parameters, ResolutionSyntax[] resolutions)
		{
			MethodName = methodName;
			ElementType = elementType;
			Parameters = parameters;
			Resolutions = resolutions;
		}

		public static CollectionResolverSyntax? FromResolver(IMethodSymbol resolver)
		{
			ResolutionSyntax? GetResolutionAsElement(AttributeData resolutionAttr, TypeName type)
			{
				if (resolutionAttr.ConstructorArguments[0].Value is INamedTypeSymbol nts)
				{
					if (TypeName.FromSymbol(nts.BaseType) == type)
					{
						return ResolutionSyntax.FromType(nts);
					}

					foreach (var @interface in nts.AllInterfaces)
					{
						if (TypeName.FromSymbol(@interface) == type)
						{
							return ResolutionSyntax.FromType(nts);
						}
					}
				}

				return null;
			}

			var elementType = TypeName.FromSymbol(resolver.ReturnType);

			var parameters = ParameterSyntax.FromResolver(resolver);

			var resolutions = resolver.GetAttributes()
				.Where(x => x.AttributeClass.Name == nameof(ResolutionAttribute))
				.Where(x => x.AttributeConstructor.Parameters.Length == 1)
				.Select(x => GetResolutionAsElement(x, elementType))
				.FilterNull()
				.ToArray();

			return new CollectionResolverSyntax(resolver.Name, elementType, parameters, resolutions);
		}
	}
}
