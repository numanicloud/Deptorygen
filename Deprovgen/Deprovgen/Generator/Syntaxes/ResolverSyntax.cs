using System.Collections.Generic;
using System.Linq;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Syntaxes
{
	class ResolverSyntax
	{
		public string MethodName { get; }
		public TypeName ReturnTypeName { get; }
		public ResolutionSyntax? ReturnTypeResolution { get; }
		public ResolutionSyntax[] Resolutions { get; }
		public ParameterSyntax[] Parameters { get; }
		public Accessibility Accessibility { get; set; }

		public ResolverSyntax(string methodName,
			TypeName returnTypeName,
			ResolutionSyntax? returnTypeResolution,
			ResolutionSyntax[] resolutions,
			ParameterSyntax[] parameters, Accessibility accessibility)
		{
			MethodName = methodName;
			ReturnTypeName = returnTypeName;
			ReturnTypeResolution = returnTypeResolution;
			Resolutions = resolutions;
			Parameters = parameters;
			Accessibility = accessibility;
		}

		public static (ResolverSyntax[], CollectionResolverSyntax[]) FromParent(INamedTypeSymbol factory)
		{
			var baseInterfaces = factory.AllInterfaces
				.SelectMany(x => x.GetMembers())
				.OfType<IMethodSymbol>();

			var resolvers = factory.GetMembers()
				.OfType<IMethodSymbol>()
				.Concat(baseInterfaces)
				.Where(x => x.MethodKind == MethodKind.Ordinary)
				.ToArray();

			var resolverResult = new List<ResolverSyntax>();
			var collectionResult = new List<CollectionResolverSyntax>();

			foreach (var item in resolvers)
			{
				if (CollectionResolverSyntax.FromResolver(item) is {} cr)
				{
					collectionResult.Add(cr);
				}
				else if(item.ReturnType is INamedTypeSymbol returnType)
				{
					var r = new ResolverSyntax(
						item.Name,
						TypeName.FromSymbol(returnType),
						ResolutionSyntax.FromType(returnType),
						ResolutionSyntax.FromResolversAttribute(item),
						ParameterSyntax.FromResolver(item),
						item.DeclaredAccessibility);
					resolverResult.Add(r);
				}
			}

			return (resolverResult.ToArray(), collectionResult.ToArray());
		}
	}
}
