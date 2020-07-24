using System.Linq;
using Deprovgen.Annotations;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Syntaxes
{
	class ResolutionSyntax
	{
		public TypeName TypeName { get; }
		public TypeName[] Dependencies { get; set; }

		public ResolutionSyntax(TypeName typeName, TypeName[] dependencies)
		{
			TypeName = typeName;
			Dependencies = dependencies;
		}

		public static ResolutionSyntax FromType(INamedTypeSymbol symbol)
		{
			var dependencies = symbol.Constructors[0].Parameters
				.Select(x => TypeName.FromSymbol(x.Type))
				.ToArray();
			return new ResolutionSyntax(TypeName.FromSymbol(symbol), dependencies);
		}

		public static ResolutionSyntax[] FromResolversAttribute(IMethodSymbol resolver)
		{
			return resolver.GetAttributes()
				.Where(x => x.AttributeClass.Name == nameof(ResolutionAttribute))
				.Where(x => x.AttributeClass.Constructors.Length == 1)
				.Where(x => x.AttributeClass.Constructors[0].Parameters.Length == 1)
				.Select(x => x.ConstructorArguments[0].Value)
				.OfType<INamedTypeSymbol>()
				.Select(FromType)
				.ToArray();
		}
	}
}
