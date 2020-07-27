using System.Collections.Generic;
using System.Linq;
using Deprovgen.Annotations;
using Deprovgen.Generator.Definition;
using Deprovgen.Generator.Syntaxes;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.Analyzer
{
	class FactoryAnalyzer
	{
		private readonly FactorySyntax _syntax;

		public FactoryAnalyzer(FactorySyntax syntax)
		{
			_syntax = syntax;
		}

		public FactoryDefinition GetDefinition()
		{
			var interfaceName = _syntax.InterfaceSymbol.Name;
			var typeName = interfaceName[0].ToString().Replace("I", "") +
			               interfaceName.Substring(1);

			var genericHost = _syntax.InterfaceSymbol.HasAttribute(nameof(ConfigureGenericHostAttribute));

			var resolvers = _syntax.Resolvers
				.Select(x => new ResolverAnalyzer(x).GetDefinition())
				.ToArray();

			var collectionResolvers = _syntax.CollectionResolvers
				.Select(x => new CollectionResolverAnalyzer(x).GetDefinition())
				.ToArray();

			var captures = _syntax.Captures
				.Select(x => new CaptureAnalyzer(x).GetDefinition())
				.ToArray();

			return new FactoryDefinition(
				typeName,
				TypeName.FromSymbol(_syntax.InterfaceSymbol),
				GetDependencies(),
				resolvers,
				collectionResolvers,
				captures,
				genericHost);
		}

		private TypeName[] GetDependencies()
		{
			static IEnumerable<TypeName> ExceptParameters(IEnumerable<ResolutionSyntax> source, ParameterSyntax[] parameters)
			{
				return source.SelectMany(x => x.Dependencies)
					.Except(parameters.Select(x => x.TypeName));
			}

			var returnDeps = _syntax.Resolvers
				.SelectMany(x => ExceptParameters(x.ReturnTypeResolution.AsEnumerable(), x.Parameters));

			var resolutionDeps = _syntax.Resolvers
				.SelectMany(x => ExceptParameters(x.Resolutions, x.Parameters));

			var collectionDeps = _syntax.CollectionResolvers
				.SelectMany(x => ExceptParameters(x.Resolutions, x.Parameters));

			var captureAbilities = _syntax.Captures
				.SelectMany(x => x.Resolvers)
				.Select(x => x.ReturnTypeName);

			var captureCollectionAbilities = _syntax.Captures
				.SelectMany(x => x.CollectionResolvers)
				.Select(x => x.CollectionType);

			var resolverAbilities = _syntax.Resolvers
				.Select(x => x.ReturnTypeName);

			var collectionAbilities = _syntax.CollectionResolvers
				.Select(x => x.CollectionType);

			var self = new TypeName[]
			{
				TypeName.FromSymbol(_syntax.InterfaceSymbol),
			};

			return returnDeps.Concat(resolutionDeps)
				.Concat(collectionDeps)
				.Except(captureAbilities)
				.Except(captureCollectionAbilities)
				.Except(resolverAbilities)
				.Except(collectionAbilities)
				.Except(self)
				.ToArray();
		}
	}
}
