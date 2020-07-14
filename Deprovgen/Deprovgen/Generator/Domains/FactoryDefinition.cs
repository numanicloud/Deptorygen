using System.Collections.Generic;
using System.Linq;

namespace Deprovgen.Generator.Domains
{
	public class FactoryDefinition
	{
		public FactoryDefinition(string nameSpace, string typeName, ServiceDefinition[] dependencies, ResolverDefinition[] resolvers, FactoryDefinition[] children, string interfaceName)
		{
			NameSpace = nameSpace;
			TypeName = typeName;
			Dependencies = dependencies;
			Resolvers = resolvers;
			Children = children;
			InterfaceName = interfaceName;
		}

		public string NameSpace { get; }
		public string TypeName { get; }
		public string InterfaceName { get; }
		public ServiceDefinition[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public FactoryDefinition[] Children { get; }

		public override string ToString()
		{
			return $"{NameSpace}.{TypeName}; dependencies x{Dependencies.Length}, resolvers x{Resolvers.Length}, children x{Children.Length}";
		}

		public string[] GetRequiredNamespaces()
		{
			var children = Children.Select(x => x.NameSpace);
			var dependencies = Dependencies.Select(x => x.Namespace);
			var resolverReturns = Resolvers.Select(x => x.ServiceType.Namespace);
			var resolverParams = Resolvers.SelectMany(x => x.Parameters)
				.Select(x => x.TypeNamespace);
			return children.Concat(dependencies)
				.Concat(resolverReturns)
				.Concat(resolverParams)
				.Distinct()
				.Where(x => x != NameSpace)
				.ToArray();
		}

		public string GetConstructorParameterList()
		{
			return Dependencies.Select(x => $"{x.TypeName} {x.ParameterName}")
				.Join(", ");
		}

		public VariableDefinition[] GetResolverParameters(FactoryDefinition parent)
		{
			return Dependencies.Where(x => parent.Dependencies.All(y => y.TypeName != x.TypeName))
				.Where(x => parent.Resolvers.All(y => y.ServiceType.TypeName != x.TypeName))
				.Select(x => new VariableDefinition(x.TypeName, x.ParameterName, x.Namespace))
				.ToArray();
		}

		public string GetResolverParameterList(FactoryDefinition parent)
		{
			return GetResolverParameters(parent).Select(x => x.Code).Join(", ");
		}

		public string GetResolverArgList(FactoryDefinition parent)
		{
			List<string> args = new List<string>();
			var parameters = GetResolverParameters(parent);

			foreach (var dependency in Dependencies)
			{
				if (parameters.FirstOrDefault(x => x.TypeName == dependency.TypeName) is { } p)
				{
					args.Add(p.VarName);
				}
				else if (parent.CalcArgForService(dependency.TypeName, parameters) is { } arg)
				{
					args.Add(arg);
				}
				else
				{
					args.Add(dependency.ParameterName);
				}
			}
			return args.Join(", ");
		}

		public string CalcArgForService(string typeName, VariableDefinition[] contextVariables)
		{
			if (Dependencies.FirstOrDefault(x => x.TypeName == typeName) is { } dep)
			{
				return dep.FieldName;
			}
			else if(Resolvers.FirstOrDefault(x => x.ServiceType.TypeName == typeName) is { } resolver)
			{
				return $"{resolver.MethodName}({resolver.GetArgList(this, contextVariables)})";
			}
			return null;
		}
	}
}
