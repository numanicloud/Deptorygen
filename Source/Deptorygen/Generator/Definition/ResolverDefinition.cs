using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class ResolverDefinition : IDefinitionRequiringNamespace, IInjectionGenerator, IAccessibilityClaimer
	{
		public string MethodName { get; }
		public TypeName ReturnType { get; }
		public ResolutionDefinition Resolution { get; }
		public VariableDefinition[] Parameters { get; }
		public bool IsTransient { get; }
		public string CacheVarName { get; }
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

		// TODO: このメソッドはResolutionDefinitionに移した方が自然かも

		public string GetArgsListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => x.VarName ?? context.GetExpression(x.TypeNameInfo))
				.Join(", ");
		}

		public bool IsAlternatedByCapture(FactoryDefinition definition)
		{
			return definition.Captures.Any(x => x.Resolvers.Any(y => y.ReturnType == ReturnType));
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
			yield return Resolution.TypeName.FullNamespace;
			foreach (var p in Parameters)
			{
				yield return p.TypeNamespace;
			}
		}

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			return typeName == ReturnType ? $"{MethodName}({GetArgsListForSelf(context)})" : null;
		}

		public IEnumerable<InjectionExpression> GetInjectionExpressions(TypeName typeName, InjectionContext context)
		{
			if (typeName == ReturnType)
			{
				yield return new InjectionExpression(typeName,
					InjectionMethod.Resolver,
					$"{MethodName}({GetArgsListForSelf(context)})");
			}
		}

		public InjectionExpression? GetDelegation(TypeName typeName, FactoryDefinition factory)
		{
			if (typeName != ReturnType) return null;

			var args = Parameters
				.Select(x => factory.GetInjectionCapabilities(x.TypeNameInfo))
				.Where(x => x.Any())
				.Select(x => x.OrderBy(y => y.Method).First().Code)
				.Join(", ");

			return new InjectionExpression(typeName,
				InjectionMethod.Resolver,
				$"{MethodName}({args})");
		}

		public IEnumerable<InjectionExpression> GetInjectionCapabilities(TypeName typeName, FactoryDefinition factory)
		{
			foreach (var capability in factory.GetInjectionCapabilities(typeName))
			{
				yield return capability;
			}

			foreach (var parameter in Parameters)
			{
				if (parameter.TypeNameInfo == typeName)
				{
					yield return new InjectionExpression(typeName, InjectionMethod.Parameter, parameter.VarName);
				}
			}
		}

		public IEnumerable<Accessibility> Accessibilities
		{
			get
			{
				yield return ReturnType.Accessibility;
				foreach (var parameter in Parameters)
				{
					yield return parameter.TypeNameInfo.Accessibility;
				}
			}
		}
	}
}
