using System;
using System.Linq;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Syntaxes;

namespace Deptorygen.Generator.Analyzer
{
	class Hoge : IDisposable
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

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

			var resolution = _syntax.Resolutions
				.Select(x => new ResolutionDefinition(_syntax.ReturnTypeName, x.TypeName, x.Dependencies, x.IsDisposable))
				.FirstOrDefault();

			if (resolution is null)
			{
				if (_syntax.ReturnTypeResolution is {} ret)
				{
					resolution = new ResolutionDefinition(_syntax.ReturnTypeName, ret.TypeName, ret.Dependencies, ret.IsDisposable);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}

			return new ResolverDefinition(
				_syntax.MethodName,
				_syntax.ReturnTypeName,
				resolution,
				parameters,
				_syntax.MethodName.EndsWith("AsTransient"),
				"_" + _syntax.MethodName + "Cache",
				_syntax.DelegationKey);
		}
	}
}
