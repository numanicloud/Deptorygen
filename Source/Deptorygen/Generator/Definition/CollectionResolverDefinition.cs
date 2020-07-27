using System;
using System.Linq;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class CollectionResolverDefinition
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

			Injection = new InjectionContext();
			foreach (var parameter in Parameters)
			{
				Injection[parameter.TypeNameInfo] = parameter.VarName;
			}
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code).Join(", ");
		}

		public string GetArgListForSelf(InjectionContext context)
		{
			return Parameters
				.Select(x => context[x.TypeNameInfo] ?? x.VarName)
				.Join(", ");
		}

		public string GetElementList(InjectionContext context)
		{
			var merged = Injection.Merge(context);
			return ServiceTypes.Select(x => merged[x] ?? x.LowerCamelCase)
				.Join("," + Environment.NewLine + "\t\t\t\t");
		}
	}
}
