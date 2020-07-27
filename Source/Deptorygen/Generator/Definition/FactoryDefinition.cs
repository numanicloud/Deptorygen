using System.Collections.Generic;
using System.Linq;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class FactoryDefinition
	{
		public string TypeName { get; }
		public TypeName InterfaceNameInfo { get; }
		public TypeName[] Dependencies { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public CaptureDefinition[] Captures { get; }
		public bool DoSupportGenericHost { get; }
		public InjectionContext Injection { get; }
		public InjectionContext CapturedInjection { get; }

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

			Injection = CreateInjectionContext();
			CapturedInjection = Captures.Aggregate(new InjectionContext(),
				(accumlate, x) => x.Injection.Merge(accumlate));
		}

		private InjectionContext CreateInjectionContext()
		{
			var injection = new InjectionContext
			{
				[InterfaceNameInfo] = "this"
			};

			foreach (var dependency in Dependencies)
			{
				injection[dependency] = $"_{dependency.LowerCamelCase}";
			}

			foreach (var resolver in Resolvers)
			{
				injection[resolver.ReturnType] = $"{resolver.MethodName}({resolver.GetArgsListForSelf(injection)})";
			}

			foreach (var resolver in CollectionResolvers)
			{
				injection[resolver.ReturnType] = $"{resolver.MethodName}({resolver.GetArgListForSelf(injection)})";
			}

			foreach (var capture in Captures)
			{
				injection = injection.Merge(capture.Injection);
			}

			return injection;
		}

		public string GetConstructorParameterList()
		{
			var deps = Dependencies.Select(x => $"{x.Name} {x.LowerCamelCase}");
			var caps = Captures.Select(x => $"{x.InterfaceNameInfo.Name} {x.ParameterName}");
			return deps.Concat(caps).Join(", ");
		}

		public string[] GetRequiredNamespaces()
		{
			IEnumerable<IEnumerable<string>> GetNamespaces()
			{
				yield return new[]
				{
					"System",
					"System.Collections.Generic"
				};

				yield return Resolvers.Select(x => x.ReturnType.FullNamespace);
				yield return Resolvers.SelectMany(x => x.Parameters)
					.Select(x => x.TypeNamespace);
				yield return CollectionResolvers.Select(x => x.ElementTypeInfo.FullNamespace);
				yield return CollectionResolvers.SelectMany(x => x.Parameters)
					.Select(x => x.TypeNamespace);
				yield return Dependencies.Select(x => x.FullNamespace);
				yield return Captures.Select(x => x.InterfaceNameInfo.FullNamespace);

				if (DoSupportGenericHost)
				{
					yield return new[]
					{
						"Deprovgen.GenericHost",
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
			var accessibilities = Captures.Select(x => x.InterfaceNameInfo)
				.Concat(Dependencies)
				.Select(x => x.Accessibility)
				.Concat(Resolvers.Select(x => x.Accessibility))
				.Concat(CollectionResolvers.Select(x => x.MostStrictAccessibility));

			return accessibilities.Any(x => x != Accessibility.Public) ? "internal" : "public";
		}

		public (string typeName, string expression)[] GetResolverExpressionsForGenericHost()
		{
			return Resolvers.Where(x => x.Parameters.Length == 0)
				.Select(x => (x.ReturnType.Name, $"{x.MethodName}()"))
				.ToArray();
		}
	}
}
