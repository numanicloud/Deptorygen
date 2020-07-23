using System.Collections.Generic;
using System.Linq;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.Domains
{
	public class ServiceDefinition
	{
		public ServiceDefinition(TypeName typeName, ServiceDefinition[] dependencies)
		{
			TypeNameInfo = typeName;
			Dependencies = dependencies;
		}

		public TypeName TypeNameInfo { get; }
		public string Namespace => TypeNameInfo.FullNamespace;
		public string TypeName => TypeNameInfo.Name;
		public string FieldName => "_" + TypeNameInfo.LowerCamelCase;
		public string ParameterName => TypeNameInfo.LowerCamelCase;
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
