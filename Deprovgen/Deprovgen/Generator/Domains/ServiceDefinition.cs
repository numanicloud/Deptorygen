using System.Collections.Generic;
using System.Linq;

namespace Deprovgen.Generator.Domains
{
	public class ServiceDefinition
	{
		public ServiceDefinition(string ns,
			string typeName,
			string fieldName,
			string parameterName,
			ServiceDefinition[] dependencies)
		{
			Namespace = ns;
			TypeName = typeName;
			FieldName = fieldName;
			ParameterName = parameterName;
			Dependencies = dependencies;
		}

		public string Namespace { get; }
		public string TypeName { get; }
		public string FieldName { get; }
		public string ParameterName { get; }
		public ServiceDefinition[] Dependencies { get; }

		public override string ToString()
		{
			return $"{Namespace}.{TypeName}; dependencies x{Dependencies.Length}";
		}

		public string GetConstructorArgList(VariableDefinition[] resolverParameters, FactoryDefinition owner)
		{
			List<string> parameters = new List<string>();
			foreach (var dependency in Dependencies)
			{
				if (resolverParameters.FirstOrDefault(x => x.TypeName == dependency.TypeName) is { } def)
				{
					parameters.Add(def.VarName);
				}
				else if(owner.CalcArgForService(dependency.TypeName, resolverParameters) is { } arg)
				{
					parameters.Add(arg);
				}
				else
				{
					parameters.Add(dependency.FieldName);
				}
			}
			return parameters.Join(", ");
		}
	}
}
