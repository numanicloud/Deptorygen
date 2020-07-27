using System.Linq;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Syntaxes;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Analyzer
{
	class CollectionResolverAnalyzer
	{
		private readonly CollectionResolverSyntax _syntax;

		public CollectionResolverAnalyzer(CollectionResolverSyntax syntax)
		{
			_syntax = syntax;
		}

		public CollectionResolverDefinition GetDefinition()
		{
			var accessibility = _syntax.Parameters.Select(x => x.TypeName.Accessibility)
				.Append(_syntax.ElementType.Accessibility)
				.Aggregate((accum, x) => x != Accessibility.Public ? Accessibility.Internal : accum);

			var resolutions = _syntax.Resolutions
				.Select(x => x.TypeName)
				.ToArray();

			var parameters = _syntax.Parameters
				.Select(x => new VariableDefinition(x.TypeName, x.ParameterName))
				.ToArray();

			return new CollectionResolverDefinition(
				_syntax.CollectionType,
				_syntax.MethodName,
				resolutions,
				parameters,
				accessibility);
		}
	}
}
