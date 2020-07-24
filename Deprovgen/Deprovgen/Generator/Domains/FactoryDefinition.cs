using System;
using System.Collections.Generic;
using System.Linq;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Domains
{
	public class FactoryDefinition
	{
		public FactoryDefinition(string typeName,
			TypeName interfaceNameInfo,
			TypeName[] dependencies,
			ResolverDefinition[] resolvers,
			bool doSupportGenericHost,
			CaptureDefinition[] captures,
			CollectionResolverDefinition[] collectionResolvers)
		{
			TypeName = typeName;
			InterfaceNameInfo = interfaceNameInfo;
			Dependencies = dependencies;
			Resolvers = resolvers;
			DoSupportGenericHost = doSupportGenericHost;
			Captures = captures;
			CollectionResolvers = collectionResolvers;
		}

		public string NameSpace => InterfaceNameInfo.FullNamespace;
		public string TypeName { get; }
		public string InterfaceName => InterfaceNameInfo.Name;
		public TypeName InterfaceNameInfo { get; }
		public TypeName[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; set; }
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

				yield return Dependencies.Select(x => x.FullNamespace);
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
				.Select(x => $"{x.Name} {x.LowerCamelCase}");
			var caps = Captures
				.Select(x => $"{x.InterfaceName} {x.ParameterName}");

			return deps.Concat(caps).Join(", ");
		}

		public string CalcArgForService(string typeName, VariableDefinition[] contextVariables)
		{
			if (typeName == TypeName || typeName == InterfaceNameInfo)
			{
				return "this";
			}

			if (Dependencies.FirstOrDefault(x => x.Name == typeName) is { } dep)
			{
				return "_" + dep.LowerCamelCase;
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
