using System;
using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class CollectionResolverDefinition : IDefinitionRequiringNamespace, IInjectionGenerator, IAccessibilityClaimer
	{
		public TypeName ReturnType { get; }
		public TypeName ElementTypeInfo => ReturnType.TypeArguments[0];
		public string MethodName { get; }
		public TypeName[] ServiceTypes { get; }
		public VariableDefinition[] Parameters { get; }
		public Accessibility MostStrictAccessibility { get; }
		public string ElementTypeName => ElementTypeInfo.Name;
		public InjectionContext Injection { get; }

		public CollectionResolverDefinition(TypeName returnType,
			string methodName,
			TypeName[] serviceTypes,
			VariableDefinition[] parameters,
			Accessibility mostStrictAccessibility)
		{
			ReturnType = returnType;
			MethodName = methodName;
			ServiceTypes = serviceTypes;
			Parameters = parameters;
			MostStrictAccessibility = mostStrictAccessibility;

			var store = new InjectionStore();
			foreach (var parameter in Parameters)
			{
				store[parameter.TypeNameInfo] = parameter.VarName;
			}
			Injection = new InjectionContext(new []{store});
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code).Join(", ");
		}

		public string GetArgListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => x.VarName)
				.Join(", ");
		}

		public string GetElementList(InjectionContext context)
		{
			var merged = Injection.Merge(context);
			return ServiceTypes.Select(x => merged.GetExpression(x) ?? x.LowerCamelCase)
				.Join("," + Environment.NewLine + "\t\t\t\t");
		}

		public IEnumerable<string> GetRequiredNamespaces()
		{
			yield return ElementTypeInfo.FullNamespace;
			foreach (var p in Parameters)
			{
				yield return p.TypeNamespace;
			}
		}

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			return typeName == ReturnType ? $"{MethodName}({GetArgListForSelf(context)})" : null;
		}

		public IEnumerable<InjectionExpression> GetInjectionExpressions(TypeName typeName, InjectionContext context)
		{
			if (typeName == ReturnType)
			{
				yield return new InjectionExpression(typeName,
					InjectionMethod.Resolver,
					$"{MethodName}({GetArgListForSelf(context)})");
			}
		}

		public IEnumerable<Accessibility> Accessibilities
		{
			get
			{
				yield return ElementTypeInfo.Accessibility;
				foreach (var parameter in Parameters)
				{
					yield return parameter.TypeNameInfo.Accessibility;
				}
			}
		}
	}
}
