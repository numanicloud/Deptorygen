using System.Linq;
using Deprovgen.Annotations;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Syntaxes
{
	class ResolutionSyntax
	{
		public TypeName TypeName { get; }
		public TypeName[] Dependencies { get; }
		public bool IsDisposable { get; }

		public ResolutionSyntax(TypeName typeName, TypeName[] dependencies, bool isDisposable)
		{
			TypeName = typeName;
			Dependencies = dependencies;
			IsDisposable = isDisposable;
		}

		public static ResolutionSyntax FromType(INamedTypeSymbol symbol)
		{
			var isDisposable = symbol.AllInterfaces
				.Any(x => x.Name == "IDisposable" && x.GetFullNameSpace() == "System");

			if (symbol.Constructors.Length == 0)
			{
				return new ResolutionSyntax(TypeName.FromSymbol(symbol), new TypeName[0], isDisposable);
			}

			var dependencies = symbol.Constructors[0].Parameters
				.Select(x => TypeName.FromSymbol(x.Type))
				.ToArray();
			return new ResolutionSyntax(TypeName.FromSymbol(symbol), dependencies, isDisposable);
		}

		public static ResolutionSyntax[] FromResolversAttribute(IMethodSymbol resolver)
		{
			return resolver.GetAttributes()
				.Where(x =>
				{
					Logger.WriteLine($"Check {x} is ResolutionAttribute").Wait();
					return x.AttributeClass.Name == nameof(ResolutionAttribute);
				})
				.Where(x =>
				{
					Logger.WriteLine($"Check {x} has constructor with 1 argument").Wait();
					return x.ConstructorArguments.Length == 1;
				})
				.Select(x => x.ConstructorArguments[0].Value)
				.OfType<INamedTypeSymbol>()
				.Select(FromType)
				.ToArray();
		}
	}
}
