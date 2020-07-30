using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class ResolverDefinition : IDefinitionRequiringNamespace, IInjectionGenerator
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

		public ResolverDefinition(string methodName,
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

			var store = new InjectionStore();
			foreach (var parameter in Parameters)
			{
				store[parameter.TypeNameInfo] = $"{parameter.VarName}";
			}
			Injection = new InjectionContext(new []{store});
		}

		public bool GetRequireDispose(FactoryDefinition factory)
		{
			return Resolution.IsDisposable
			       && !IsTransient
			       && !TryGetDelegation(factory, out _);
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code).Join(", ");
		}

		public string GetInstantiationArgList(InjectionContext context)
		{
			var finalInjection = Injection.Merge(context);

			return Resolution.Dependencies
				.Select(x => finalInjection.GetExpression(x) ?? x.LowerCamelCase)
				.ToList().Join(", ");
		}

		public string GetArgsListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => context.GetExpression(x.TypeNameInfo) ?? x.VarName)
				.Join(", ");
		}

		public bool TryGetDelegation(FactoryDefinition factory, out string code)
		{
			foreach (var capture in factory.Captures)
			{
				if (capture.GetInjectionExpression(this, factory.Injection) is {} injection)
				{
					code = injection;
					return true;
				}
			}

			code = "";
			return false;
		}

		public IEnumerable<string> GetRequiredNamespaces()
		{
			yield return ReturnType.FullNamespace;
			foreach (var p in Parameters)
			{
				yield return p.TypeNamespace;
			}
		}

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			return typeName == ReturnType ? $"{MethodName}({GetArgsListForSelf(context)})" : null;
		}
	}
}
