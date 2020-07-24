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
	public enum ResolverBehavior
	{
		Cached, Transient, Delegation
	}

	public class ResolverDefinitionV2
	{
		public string MethodName { get; }
		public TypeName ReturnType { get; }
		public ResolutionDefinition Resolution { get; }
		public VariableDefinition[] Parameters { get; }
		public bool IsTransient { get; }
		public string CacheVarName { get; }
		public Accessibility Accessibility => ReturnType.Accessibility;
		public InjectionContext Injection { get; }
		public string ResolutionName => Resolution.TypeName.Name;

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

		public string GetInstantiationArgList(InjectionContext context)
		{
			var finalInjection = Injection.Merge(context);

			return Resolution.Dependencies
				.Select(x => finalInjection[x] ?? x.LowerCamelCase)
				.ToList().Join(", ");
		}

		public string GetArgsListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => context[x.TypeNameInfo] ?? x.VarName)
				.Join(", ");
		}

		public bool TryGetDelegation(FactoryDefinitionV2 factory, out string code)
		{
			foreach (var capture in factory.Captures)
			{
				foreach (var resolver in capture.Resolvers)
				{
					if (resolver.Resolution.TypeName == Resolution.TypeName
						&& resolver.IsTransient == IsTransient)
					{
						code = $"{capture.PropertyName}.{resolver.MethodName}({resolver.GetArgsListForSelf(factory.Injection)})";
						return true;
					}
				}
			}

			code = "";
			return false;
		}
	}
}
