using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Generator.DefinitionV2;
using Deprovgen.Generator.Domains;
using Deprovgen.Generator.Syntaxes;

namespace Deprovgen.Generator.AnalyzerV2
{
	class ResolverAnalyzerV2
	{
		private readonly ResolverSyntax _syntax;

		public ResolverAnalyzerV2(ResolverSyntax syntax)
		{
			_syntax = syntax;
		}

		public ResolverDefinitionV2 GetDefinition()
		{
			var parameters = _syntax.Parameters
				.Select(x => new VariableDefinition(x.TypeName, x.ParameterName))
				.ToArray();

			var resolutions = _syntax.Resolutions
				.Select(x => new ResolutionDefinition(x.TypeName, x.Dependencies))
				.FirstOrDefault();

			if (resolutions is null)
			{
				if (_syntax.ReturnTypeResolution is {} ret)
				{
					resolutions = new ResolutionDefinition(ret.TypeName, ret.Dependencies);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}

			return new ResolverDefinitionV2(
				_syntax.MethodName,
				_syntax.ReturnTypeName,
				resolutions,
				parameters,
				_syntax.MethodName.EndsWith("AsTransient"),
				"_" + _syntax.MethodName + "Cache");
		}
	}
}
