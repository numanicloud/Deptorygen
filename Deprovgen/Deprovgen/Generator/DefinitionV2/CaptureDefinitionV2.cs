using System;
using System.Collections.Generic;
using System.Text;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.DefinitionV2
{
	class CaptureDefinitionV2
	{
		public TypeName InterfaceName { get; }
		public ResolverDefinitionV2[] Resolvers { get; }
		public CollectionResolverDefinitionV2[] CollectionResolvers { get; }
		public InjectionContext Injection { get; }
		public string PropertyName { get; }

		public CaptureDefinitionV2(TypeName interfaceName,
			string propertyName,
			ResolverDefinitionV2[] resolvers,
			CollectionResolverDefinitionV2[] collectionResolvers)
		{
			InterfaceName = interfaceName;
			PropertyName = propertyName;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;

			Injection = new InjectionContext();
			foreach (var resolver in Resolvers)
			{
				Injection[resolver.ReturnType] = $"{propertyName}.{resolver.MethodName}({resolver.GetArgsListForSelf(Injection)})";
			}

			foreach (var resolver in collectionResolvers)
			{
				var typeName = new TypeName(
					resolver.ElementTypeInfo.FullNamespace,
					$"IEnumerable<{resolver.ElementTypeInfo.Name}>",
					resolver.ElementTypeInfo.Accessibility);
				Injection[typeName] = $"{propertyName}.{resolver.MethodName}({resolver.GetArgListForSelf(Injection)})";
			}
		}
	}
}
