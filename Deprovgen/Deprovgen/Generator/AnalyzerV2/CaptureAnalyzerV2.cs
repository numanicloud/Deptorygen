using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Generator.DefinitionV2;
using Deprovgen.Generator.Syntaxes;

namespace Deprovgen.Generator.AnalyzerV2
{
	class CaptureAnalyzerV2
	{
		private readonly CaptureSyntax _syntax;

		public CaptureAnalyzerV2(CaptureSyntax syntax)
		{
			_syntax = syntax;
		}

		public CaptureDefinitionV2 GetDefinition()
		{
			var resolvers = _syntax.Resolvers
				.Select(x => new ResolverAnalyzerV2(x).GetDefinition())
				.ToArray();

			var collectionResolvers = _syntax.CollectionResolvers
				.Select(x => new CollectionResolverAnalyzerV2(x).GetDefinition())
				.ToArray();

			return new CaptureDefinitionV2(
				_syntax.TypeName,
				_syntax.PropertyName,
				resolvers,
				collectionResolvers);
		}
	}
}
