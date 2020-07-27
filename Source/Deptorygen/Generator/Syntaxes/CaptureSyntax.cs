using System.Collections.Generic;
using System.Linq;
using Deptorygen.Annotations;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Syntaxes
{
	class CaptureSyntax
	{
		public string PropertyName { get; }
		public TypeName TypeName { get; }
		public ResolverSyntax[] Resolvers { get; }
		public CollectionResolverSyntax[] CollectionResolvers { get; }

		public CaptureSyntax(string propertyName, TypeName typeName, ResolverSyntax[] resolvers, CollectionResolverSyntax[] collectionResolvers)
		{
			PropertyName = propertyName;
			TypeName = typeName;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;
		}

		public static CaptureSyntax[] FromFactory(INamedTypeSymbol factory)
		{
			IEnumerable<CaptureSyntax> GetCaptures()
			{
				var inInterfaces = factory.AllInterfaces
					.SelectMany(x => x.GetMembers());

				var properties = factory.GetMembers()
					.Concat(inInterfaces)
					.OfType<IPropertySymbol>();

				foreach (var property in properties)
				{
					if (!property.IsReadOnly)
					{
						continue;
					}

					if (!property.Type.HasAttribute(nameof(FactoryAttribute)))
					{
						continue;
					}

					if (property.Type is INamedTypeSymbol namedType)
					{
						var resolvers = ResolverSyntax.FromParent(namedType);
						yield return new CaptureSyntax(property.Name,
							TypeName.FromSymbol(namedType),
							resolvers.Item1,
							resolvers.Item2);
					}
				}
			}

			return GetCaptures().ToArray();
		}
	}
}
