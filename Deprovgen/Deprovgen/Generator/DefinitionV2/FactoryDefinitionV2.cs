using System.Linq;
using System.Text.RegularExpressions;
using Deprovgen.Utilities;

namespace Deprovgen.Generator.DefinitionV2
{
	class FactoryDefinitionV2
	{
		public string TypeName { get; }
		public TypeName InterfaceNameInfo { get; }
		public TypeName[] Dependencies { get; }
		public ResolverDefinitionV2[] Resolvers { get; }
		public CollectionResolverDefinitionV2[] CollectionResolvers { get; }
		public CaptureDefinitionV2[] Captures { get; }
		public bool DoSupportGenericHost { get; }
		public InjectionContext Injection { get; }

		public FactoryDefinitionV2(string typeName,
			TypeName interfaceNameInfo,
			TypeName[] dependencies,
			ResolverDefinitionV2[] resolvers,
			CollectionResolverDefinitionV2[] collectionResolvers,
			CaptureDefinitionV2[] captures,
			bool doSupportGenericHost)
		{
			TypeName = typeName;
			InterfaceNameInfo = interfaceNameInfo;
			Dependencies = dependencies;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;
			Captures = captures;
			DoSupportGenericHost = doSupportGenericHost;

			Injection = new InjectionContext
			{
				[interfaceNameInfo] = "this"
			};

			foreach (var dependency in dependencies)
			{
				Injection[dependency] = $"_{dependency.LowerCamelCase}";
			}

			foreach (var resolver in resolvers)
			{
				Injection[resolver.ReturnType] = $"{resolver.MethodName}({resolver.GetArgsListForSelf(Injection)})";
			}

			foreach (var resolver in collectionResolvers)
			{
				var collectionTypeName = new TypeName(
					resolver.ElementTypeInfo.FullNamespace,
					$"IEnumerable<{resolver.ElementTypeInfo.Name}>",
					resolver.ElementTypeInfo.Accessibility);
				Injection[collectionTypeName] = $"{resolver.MethodName}({resolver.GetArgListForSelf(Injection)})";
			}

			foreach (var capture in captures)
			{
				Injection = Injection.Merge(capture.Injection);
			}
		}

		public string ConstructorParameterList()
		{
			var deps = Dependencies.Select(x => $"{x.Name} {x.LowerCamelCase}");
			var caps = Captures.Select(x =>
				$"{x.InterfaceName.Name} {x.InterfaceName.ToNonInterfaceName().LowerCamelCase}");
			return deps.Concat(caps).Join(", ");
		}
	}
}
