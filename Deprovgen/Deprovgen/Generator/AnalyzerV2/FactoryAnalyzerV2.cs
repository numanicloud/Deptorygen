using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Annotations;
using Deprovgen.Generator.DefinitionV2;
using Deprovgen.Generator.Domains;
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
			var returnDeps = _syntax.Resolvers
				.SelectMany(x =>
				{
					var deps = x.ReturnTypeResolution?.Dependencies ?? new TypeName[0];
					return deps.Except(x.Parameters.Select(y => y.TypeName));
				});

			var resolutionDeps = _syntax.Resolvers
				.SelectMany(x =>
				{
					return x.Resolutions.SelectMany(y => y.Dependencies)
						.Except(x.Parameters.Select(y => y.TypeName));
				});

			var captureAbilities = _syntax.Captures
				.SelectMany(x => x.Resolvers)
				.Select(x => x.ReturnTypeName);

			var resolverAbilities = _syntax.Resolvers
				.Select(x => x.ReturnTypeName);

			var dependencies = returnDeps.Concat(resolutionDeps)
				.Except(captureAbilities)
				.Except(resolverAbilities)
				.ToArray();
			return dependencies;
		}
	}
}
