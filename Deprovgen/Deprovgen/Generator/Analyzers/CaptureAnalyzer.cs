using System;
using System.Collections.Generic;
using System.Text;
using Deprovgen.Generator.Domains;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Analyzers
{
	class CaptureAnalyzer
	{
		private readonly IPropertySymbol _symbol;
		private readonly INamedTypeSymbol _namedTypeSymbol;

		public CaptureAnalyzer(IPropertySymbol symbol)
		{
			_symbol = symbol;
			_namedTypeSymbol = symbol.Type is INamedTypeSymbol nts ? nts : throw new ArgumentException();
		}

		public CaptureDefinition GetDefinition()
		{
			var ns = _symbol.Type.GetFullNameSpace();

			var interfaceName = _symbol.Type.Name;

			var propertyName = _symbol.Name;

			var parameterName = propertyName.ToLowerCamelCase();

			var resolvers = ResolverAnalyzer.GetResolverDefinitions(_namedTypeSymbol);

			var accessibility = _symbol.Type.DeclaredAccessibility;

			return new CaptureDefinition(ns, interfaceName, propertyName, parameterName, resolvers, accessibility);
		}
	}
}
