using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

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

			var store = new InjectionStore();
			foreach (var capture in Captures)
			{
				store[capture.InterfaceNameInfo] = capture.PropertyName;
			}

			store[interfaceNameInfo] = "this";
			var generators = Captures.Cast<IInjectionGenerator>()
				.Concat(Resolvers)
				.Concat(CollectionResolvers)
				.Append(this)
				.ToArray();
			Injection = store.ToContext().Merge(new InjectionContext(generators));
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

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			if (typeName == InterfaceNameInfo)
			{
				return "this";
			}

			foreach (var capture in Captures)
			{
				if (typeName == capture.InterfaceNameInfo)
				{
					return capture.PropertyName;
				}
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

		public IEnumerable<InjectionExpression> GetInjectionExpressions(TypeName typeName, InjectionContext context)
		{
			yield return new InjectionExpression(typeName, InjectionMethod.This, "this");

			foreach (var capture in Captures)
			{
				if (capture.InterfaceNameInfo == typeName)
				{
					yield return new InjectionExpression(
						typeName,
						InjectionMethod.CapturedFactory,
						capture.PropertyName);
				}
			}

			foreach (var dependency in Dependencies)
			{
				if (dependency == typeName)
				{
					yield return new InjectionExpression(
						typeName,
						InjectionMethod.Field,
						$"_{dependency.LowerCamelCase}");
				}
			}
		}

		public IEnumerable<InjectionExpression> GetInjectionCapabilities(TypeName typeName)
		{
			if (typeName == InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.This, "this");
			}

			foreach (var resolver in Resolvers.Select(x => x.GetDelegation(typeName, this)).FilterNull())
			{
				yield return resolver;
			}

			foreach (var expression in CollectionResolvers.Select(x => x.GetDelegations(typeName, this)).FilterNull())
			{
				yield return expression;
			}

			foreach (var capture in Captures.SelectMany(x => x.GetDelegations(typeName, this)))
			{
				yield return capture;
			}

			foreach (var dependency in Dependencies)
			{
				if (dependency == typeName)
				{
					yield return new InjectionExpression(
						typeName,
						InjectionMethod.Field,
						$"_{dependency.LowerCamelCase}");
				}
			}
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

/*
 * 解決の優先度
 * - Parameter
 * - this
 * - Capture
 * - Dependency
 */