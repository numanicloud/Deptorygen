using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Generator.Domains;
using Deprovgen.Generator.Syntaxes;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.DefinitionV2
{
	class ResolverDefinitionV2
	{
		public string MethodName { get; }
		public TypeName ReturnType { get; }
		public ResolutionDefinition Resolution { get; }
		public VariableDefinition[] Parameters { get; }
		public bool IsTransient { get; }
		public string CacheVarName { get; }
		public Accessibility Accessibility => ReturnType.Accessibility;
		public InjectionContext Injection { get; }

		public ResolverDefinitionV2(string methodName,
			TypeName returnType,
			ResolutionDefinition resolution,
			VariableDefinition[] parameters,
			bool isTransient,
			string cacheVarName)
		{
			MethodName = methodName;
			ReturnType = returnType;
			Resolution = resolution;
			Parameters = parameters;
			IsTransient = isTransient;
			CacheVarName = cacheVarName;

			Injection = new InjectionContext();
			foreach (var parameter in Parameters)
			{
				Injection[parameter.TypeNameInfo] = $"{parameter.VarName}";
			}
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code).Join(", ");
		}

		public string GetInstantiationArgList(InjectionContext factoryInjection)
		{
			var finalInjection = factoryInjection.Merge(Injection);

			return Resolution.Dependencies
				.Select(x => finalInjection[x] ?? x.LowerCamelCase)
				.ToList().Join(", ");
		}

		public string GetArgsListForSelf(InjectionContext factoryInjection)
		{
			return Parameters
				.Select(x => factoryInjection[x.TypeNameInfo] ?? x.VarName)
				.Join(", ");
		}
	}
}
