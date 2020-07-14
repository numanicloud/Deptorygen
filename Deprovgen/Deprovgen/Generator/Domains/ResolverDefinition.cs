using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Domains
{
	public class ResolverDefinition
	{
		public string MethodName { get; }
		public ServiceDefinition ServiceType { get; }
		public VariableDefinition[] Parameters { get; }
		public bool IsTransient { get; }
		public string CacheVarName { get; }
		public Accessibility Accessibility { get; set; }

		public override string ToString()
		{
			return $"{ServiceType.Namespace}.{ServiceType.TypeName} {MethodName}; parameters x{Parameters.Length}";
		}


		public ResolverDefinition(string methodName, ServiceDefinition serviceType, VariableDefinition[] parameters, bool isTransient, Accessibility accessibility)
		{
			MethodName = methodName;
			ServiceType = serviceType;
			Parameters = parameters;
			IsTransient = isTransient;
			Accessibility = accessibility;
			CacheVarName = $"_{methodName[0].ToString().ToLower() + methodName.Substring(1)}Cache";
		}

		public string GetParameterList()
		{
			return Parameters.Select(x => x.Code)
				.Join(", ");
		}

		public string GetArgList(FactoryDefinition caller, VariableDefinition[] contextParameters)
		{
			List<string> args = new List<string>();
			foreach (var p in Parameters)
			{
				if (contextParameters.FirstOrDefault(x => x.TypeName == p.TypeName) is { } def)
				{
					args.Add(def.VarName);
				}
				else if(caller.CalcArgForService(p.TypeName, contextParameters) is { } arg)
				{
					args.Add(arg);
				}
				else
				{
					args.Add(p.VarName);
				}
			}
			return args.Join(", ");
		}
	}
}
