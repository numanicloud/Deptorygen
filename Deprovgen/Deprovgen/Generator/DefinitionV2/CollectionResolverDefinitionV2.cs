using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Generator.Domains;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.DefinitionV2
{
	class CollectionResolverDefinitionV2
	{
		public TypeName ElementTypeInfo { get; }
		public string MethodName { get; }
		public TypeName[] ServiceTypes { get; }
		public VariableDefinition[] Parameters { get; }
		public Accessibility MostStrictAccessibility { get; }

		public CollectionResolverDefinitionV2(TypeName elementTypeInfo,
			string methodName,
			TypeName[] serviceTypes,
			VariableDefinition[] parameters,
			Accessibility mostStrictAccessibility)
		{
			ElementTypeInfo = elementTypeInfo;
			MethodName = methodName;
			ServiceTypes = serviceTypes;
			Parameters = parameters;
			MostStrictAccessibility = mostStrictAccessibility;
		}

		public string GetArgListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => context[x.TypeNameInfo] ?? x.VarName)
				.Join(", ");
		}
	}
}
