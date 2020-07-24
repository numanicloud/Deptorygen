using System;
using System.Linq;
using Deprovgen.Generator.Definition;
using Deprovgen.Generator.Syntaxes;

namespace Deprovgen.Generator.Analyzer
{
	class ResolverAnalyzer
	{
		private readonly ResolverSyntax _syntax;

		public ResolverAnalyzer(ResolverSyntax syntax)
		{
			_syntax = syntax;
		}

		public ResolverDefinition GetDefinition()
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

			return new ResolverDefinition(
				_syntax.MethodName,
				_syntax.ReturnTypeName,
				resolutions,
				parameters,
				_syntax.MethodName.EndsWith("AsTransient"),
				"_" + _syntax.MethodName + "Cache");
		}
	}
}
