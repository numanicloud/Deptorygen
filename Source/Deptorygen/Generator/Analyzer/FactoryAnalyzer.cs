using System.Linq;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Generator.Syntaxes;
using Deptorygen.GenericHost;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Analyzer
{
	class FactoryAnalyzer
	{
		private readonly FactorySyntax _syntax;

		public FactoryAnalyzer(FactorySyntax syntax)
		{
			_syntax = syntax;
		}

		private string GetFactoryTypeName()
		{
			var interfaceName = _syntax.InterfaceSymbol.Name;
			return interfaceName[0].ToString().Replace("I", "") +
				interfaceName.Substring(1);
		}

		public FactoryDefinition GetDefinition()
		{
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
				GetFactoryTypeName(),
				TypeName.FromSymbol(_syntax.InterfaceSymbol),
				GetDependencies(),
				resolvers,
				collectionResolvers,
				captures,
				genericHost);
		}

		private TypeName[] GetDependencies()
		{
			var alternatives = _syntax.Captures
				.Cast<IServiceProvider>()
				.Append(_syntax)
				.SelectMany(x => x.GetCapableServiceTypes())
				.ToArray();

			var unAlternatedResolvers = _syntax.Resolvers
				.Where(x => !(x.Resolutions.Any() && alternatives.IsSuperSet(x.Resolutions.Select(y => y.TypeName))))
				.Where(x => !alternatives.Contains(x.ReturnTypeName));

			var consumed = unAlternatedResolvers
				.Cast<IServiceConsumer>()
				.Concat(_syntax.CollectionResolvers)
				.SelectMany(x => x.GetRequiredServiceTypes());

			var provided = _syntax.Resolvers
				.Cast<IServiceProvider>()
				.Concat(_syntax.CollectionResolvers)
				.Concat(_syntax.Captures)
				.Append(_syntax)
				.SelectMany(x => x.GetCapableServiceTypes())
				.ToArray();

			return consumed.Except(provided).ToArray();
		}
	}
}
