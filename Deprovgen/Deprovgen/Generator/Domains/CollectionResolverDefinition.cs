using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Domains
{
	public class CollectionResolverDefinition
	{
		public TypeName ElementTypeInfo { get; }
		public string MethodName { get; }
		public string ElementTypeName => ElementTypeInfo.Name;
		public ServiceDefinition[] ServiceTypes { get; }
		public VariableDefinition[] Parameters { get; }
		public Accessibility MostStrictAccessibility { get; }

		public CollectionResolverDefinition(TypeName elementTypeInfo, string methodName, ServiceDefinition[] serviceTypes, VariableDefinition[] parameters, Accessibility mostStrictAccessibility)
		{
			ElementTypeInfo = elementTypeInfo;
			MethodName = methodName;
			ServiceTypes = serviceTypes;
			Parameters = parameters;
			MostStrictAccessibility = mostStrictAccessibility;
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code)
				.Join(", ");
		}

		public string GetElementList(FactoryDefinition caller, VariableDefinition[] contextParameters)
		{
			List<string> elements = new List<string>();
			foreach (var service in ServiceTypes)
			{
				if (contextParameters.FirstOrDefault(x => x.TypeName == service.TypeName) is {} p)
				{
					elements.Add(p.VarName);
				}
				else if (caller.CalcArgForService(service.TypeName, contextParameters) is {} dep)
				{
					elements.Add(dep);
				}
				else
				{
					elements.Add(service.ParameterName);
				}
			}

			return elements.Join("," + Environment.NewLine);
		}

		public bool IsSameResolver(ResolverDefinition other)
		{
			if (other.MethodName != MethodName)
			{
				return false;
			}

			if (Parameters.Length != other.Parameters.Length)
			{
				return false;
			}

			for (int i = 0; i < Parameters.Length && i < other.Parameters.Length; i++)
			{
				if (Parameters[i].TypeNameInfo != other.Parameters[i].TypeNameInfo)
				{
					return false;
				}
			}

			return true;
		}
	}
}
