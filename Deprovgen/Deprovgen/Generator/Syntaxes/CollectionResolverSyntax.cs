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
		public TypeName CollectionType { get; }
		public TypeName ElementType => CollectionType.TypeArguments[0];
		public ParameterSyntax[] Parameters { get; }
		public ResolutionSyntax[] Resolutions { get; }

		public CollectionResolverSyntax(string methodName,
			TypeName collectionType,
			ParameterSyntax[] parameters,
			ResolutionSyntax[] resolutions)
		{
			MethodName = methodName;
			CollectionType = collectionType;
			Parameters = parameters;
			Resolutions = resolutions;
		}

		public static CollectionResolverSyntax? FromResolver(IMethodSymbol resolver)
		{
			static ResolutionSyntax? GetResolutionAsElement(AttributeData resolutionAttr, TypeName type)
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

			var typeName = TypeName.FromSymbol(resolver.ReturnType);
			if (typeName.NameWithoutArguments == "IEnumerable" && typeName.TypeArguments.Length == 1)
			{
				var elementType = typeName.TypeArguments[0];

				var parameters = ParameterSyntax.FromResolver(resolver);

				var resolutions = resolver.GetAttributes()
					.Where(x => x.AttributeClass.Name == nameof(ResolutionAttribute))
					.Where(x => x.AttributeConstructor.Parameters.Length == 1)
					.Select(x => GetResolutionAsElement(x, elementType))
					.FilterNull()
					.ToArray();

				if (resolutions.Any())
				{
					return new CollectionResolverSyntax(resolver.Name, typeName, parameters, resolutions);
				}
			}

			return null;
		}
	}
}
