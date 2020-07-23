using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Domains
{
	public class FactoryDefinition
	{
		public FactoryDefinition(string nameSpace,
			string typeName,
			ServiceDefinition[] dependencies,
			ResolverDefinition[] resolvers,
			string interfaceName,
			bool doSupportGenericHost,
			CaptureDefinition[] captures)
		{
			NameSpace = nameSpace;
			TypeName = typeName;
			Dependencies = dependencies;
			Resolvers = resolvers;
			InterfaceName = interfaceName;
			DoSupportGenericHost = doSupportGenericHost;
			Captures = captures;
		}

		public string NameSpace { get; }
		public string TypeName { get; }
		public string InterfaceName { get; }
		public ServiceDefinition[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CaptureDefinition[] Captures { get; set; }
		public bool DoSupportGenericHost { get; }

		public override string ToString()
		{
			return $"{NameSpace}.{TypeName}; dependencies x{Dependencies.Length}, resolvers x{Resolvers.Length}, captures x{Captures.Length}";
		}

		public string GetAccessibility()
		{
			var max = Resolvers.Select(x => x.Accessibility)
				.Concat(Captures.Select(x => x.Accessibility))
				.OrderByDescending(x => x switch
				{
					Accessibility.NotApplicable => 7,
					Accessibility.Private => 10,
					Accessibility.ProtectedAndInternal => 9,
					Accessibility.Protected => 8,
					Accessibility.Internal => 7,
					Accessibility.ProtectedOrInternal => 6,
					Accessibility.Public => 1,
					_ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
				}).First();
			return max switch
			{
				Accessibility.Public => "public",
				_ => "internal"
			};
		}

		public string[] GetRequiredNamespaces()
		{
			IEnumerable<IEnumerable<string>> GetNamespaces()
			{
				yield return new[] { "System" };

				yield return Dependencies.Select(x => x.Namespace);
				yield return Resolvers.Select(x => x.ServiceType.Namespace);
				yield return Resolvers.Select(x => x.ReturnType.GetFullNameSpace());
				yield return Resolvers.SelectMany(x => x.Parameters)
					.Select(x => x.TypeNamespace);
				yield return Captures.Select(x => x.Namespace);

				if (DoSupportGenericHost)
				{
					yield return new[]
					{
						"Deprovgen.GenericHost",
						"Microsoft.Extensions.DependencyInjection",
					};
				}
			}

			return GetNamespaces().SelectMany(x => x)
				.Distinct()
				.Where(x => x != NameSpace)
				.ToArray();
		}

		public string GetConstructorParameterList()
		{
			var deps = Dependencies
				.Select(x => $"{x.TypeName} {x.ParameterName}");
			var caps = Captures
				.Select(x => $"{x.InterfaceName} {x.ParameterName}");

			return deps.Concat(caps).Join(", ");
		}

		public string CalcArgForService(string typeName, VariableDefinition[] contextVariables)
		{
			if (typeName == TypeName || typeName == InterfaceName)
			{
				return "this";
			}

			if (Dependencies.FirstOrDefault(x => x.TypeName == typeName) is { } dep)
			{
				return dep.FieldName;
			}

			if(Resolvers.FirstOrDefault(x => x.ServiceType.TypeName == typeName) is { } resolver)
			{
				return $"{resolver.MethodName}({resolver.GetArgList(this, contextVariables)})";
			}

			if (CaptureDefinition.GetArgExpression(Captures, typeName) is {} alter)
			{
				return $"{alter.Item1.PropertyName}.{alter.Item2.MethodName}({alter.Item2.GetArgList(this, contextVariables)})";
			}

			return null;
		}
	}
}
