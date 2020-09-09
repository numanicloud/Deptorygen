using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Injection;

namespace Deptorygen.Generator.Definition
{
	public class FactoryDefinition : IAccessibilityClaimer
	{
		public string TypeName { get; }
		public TypeName InterfaceNameInfo { get; }
		public TypeName[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public CaptureDefinition[] Captures { get; }
		public bool DoSupportGenericHost { get; }

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
			var r1 = Resolvers.Where(x => x.Parameters.Length == 0)
				.Select(x => (x.ReturnType.Name, $"{x.MethodName}()"));
			return r1.ToArray();
		}

		public (string typeName, string[] expressions)[] GetCollectionResolverExpressionsForGenericHost()
		{
			var result = new List<(string, string[])>();
			foreach (var resolver in CollectionResolvers)
			{
				var elements = resolver.ServiceTypes
					.Select(x => Resolvers.FirstOrDefault(y => y.ReturnType == x))
					.FilterNull()
					.Where(x => x.Parameters.Length == 0)
					.Select(x => $"{x.MethodName}()")
					.ToArray();

				if (elements.Any())
				{
					result.Add((resolver.ElementTypeName, elements));
				}
			}

			return result.ToArray();
		}

		public IEnumerable<InjectionExpression> GetInjectionCapabilities(TypeName typeName, IResolverContext caller)
		{
			var aggregator = new InjectionAggregator();
			return aggregator.CapabilitiesFromFactory(typeName, this, caller);
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