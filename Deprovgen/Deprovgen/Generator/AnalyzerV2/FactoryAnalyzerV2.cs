using System.Collections.Generic;
using System.Linq;
using Deprovgen.Annotations;
using Deprovgen.Generator.DefinitionV2;
using Deprovgen.Generator.Syntaxes;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.AnalyzerV2
{
	class FactoryAnalyzerV2
	{
		private readonly FactorySyntax _syntax;

		public FactoryAnalyzerV2(FactorySyntax syntax)
		{
			_syntax = syntax;
		}

		public FactoryDefinitionV2 GetDefinition()
		{
			var interfaceName = _syntax.InterfaceSymbol.Name;
			var typeName = interfaceName[0].ToString().Replace("I", "") +
			               interfaceName.Substring(1);

			var genericHost = _syntax.InterfaceSymbol.HasAttribute(nameof(ConfigureGenericHostAttribute));

			var resolvers = _syntax.Resolvers
				.Select(x => new ResolverAnalyzerV2(x).GetDefinition())
				.ToArray();

			var collectionResolvers = _syntax.CollectionResolvers
				.Select(x => new CollectionResolverAnalyzerV2(x).GetDefinition())
				.ToArray();

			var captures = _syntax.Captures
				.Select(x => new CaptureAnalyzerV2(x).GetDefinition())
				.ToArray();

			return new FactoryDefinitionV2(
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

			return returnDeps.Concat(resolutionDeps)
				.Concat(collectionDeps)
				.Except(captureAbilities)
				.Except(captureCollectionAbilities)
				.Except(resolverAbilities)
				.Except(collectionAbilities)
				.ToArray();
		}
	}
}
