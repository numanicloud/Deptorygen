using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Generator.DefinitionV2;
using Deprovgen.Generator.Domains;
using Deprovgen.Generator.Syntaxes;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.AnalyzerV2
{
	class CollectionResolverAnalyzerV2
	{
		private readonly CollectionResolverSyntax _syntax;

		public CollectionResolverAnalyzerV2(CollectionResolverSyntax syntax)
		{
			_syntax = syntax;
		}

		public CollectionResolverDefinitionV2 GetDefinition()
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

			return new CollectionResolverDefinitionV2(
				_syntax.ElementType,
				_syntax.MethodName,
				resolutions,
				parameters,
				accessibility);
		}
	}
}
