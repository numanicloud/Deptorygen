using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class FactoryDefinition : IInjectionGenerator, IAccessibilityClaimer
	{
		public string TypeName { get; }
		public TypeName InterfaceNameInfo { get; }
		public TypeName[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public CaptureDefinition[] Captures { get; }
		public bool DoSupportGenericHost { get; }
		public InjectionContext Injection { get; }

		public string InterfaceName => InterfaceNameInfo.Name;
		public string NameSpace => InterfaceNameInfo.FullNamespace;

		public FactoryDefinition(string typeName,
			TypeName interfaceNameInfo,
			TypeName[] dependencies,
			ResolverDefinition[] resolvers,
			CollectionResolverDefinition[] collectionResolvers,
			CaptureDefinition[] captures,
			bool doSupportGenericHost)
		{
			TypeName = typeName;
			InterfaceNameInfo = interfaceNameInfo;
			Dependencies = dependencies;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;
			Captures = captures;
			DoSupportGenericHost = doSupportGenericHost;

			var generators = Resolvers.Cast<IInjectionGenerator>()
				.Concat(CollectionResolvers)
				.Concat(Captures)
				.Append(this)
				.ToArray();
			Injection = new InjectionContext(generators);
		}

		public string GetConstructorParameterList()
		{
			var deps = Dependencies.Select(x => $"{x.Name} {x.LowerCamelCase}");
			var caps = Captures.Select(x => $"{x.InterfaceNameInfo.Name} {x.ParameterName}");
			return deps.Concat(caps).Join(", ");
		}

		public string[] GetRequiredNamespaces()
		{
			IEnumerable<IDefinitionRequiringNamespace> GetRequesters()
			{
				return Resolvers.Cast<IDefinitionRequiringNamespace>()
					.Concat(CollectionResolvers)
					.Concat(Dependencies)
					.Concat(Captures);
			}

			IEnumerable<IEnumerable<string>> GetNamespaces()
			{
				yield return new[]
				{
					"System",
					"System.Collections.Generic"
				};

				yield return GetRequesters().SelectMany(x => x.GetRequiredNamespaces());

				if (DoSupportGenericHost)
				{
					yield return new[]
					{
						"Deptorygen.GenericHost",
						"Microsoft.Extensions.DependencyInjection"
					};
				}
			}

			return GetNamespaces().SelectMany(x => x)
				.Distinct()
				.Except(new string[] { NameSpace })
				.ToArray();
		}

		public string GetAccessibility()
		{
			var accessibilities = Captures.Cast<IAccessibilityClaimer>()
				.Append(this)
				.Concat(Resolvers)
				.Concat(CollectionResolvers)
				.SelectMany(x => x.Accessibilities);

			return accessibilities.Any(x => x != Accessibility.Public) ? "internal" : "public";
		}

		public (string typeName, string expression)[] GetResolverExpressionsForGenericHost()
		{
			return Resolvers.Where(x => x.Parameters.Length == 0)
				.Select(x => (x.ReturnType.Name, $"{x.MethodName}()"))
				.ToArray();
		}

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			if (typeName == InterfaceNameInfo)
			{
				return "this";
			}

			foreach (var dependency in Dependencies)
			{
				if (typeName == dependency)
				{
					return $"_{dependency.LowerCamelCase}";
				}
			}

			return null;
		}

		public IEnumerable<Accessibility> Accessibilities
		{
			get
			{
				// コンストラクタ引数で要求するので、アクセシビリティに影響する
				foreach (var dependency in Dependencies)
				{
					yield return dependency.Accessibility;
				}
			}
		}
	}
}
