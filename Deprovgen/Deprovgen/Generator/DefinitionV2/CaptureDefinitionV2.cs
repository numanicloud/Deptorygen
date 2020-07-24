using System;
using System.Collections.Generic;
using System.Text;
using Deprovgen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.DefinitionV2
{
	public class CaptureDefinitionV2
	{
		public TypeName InterfaceNameInfo { get; }
		public ResolverDefinitionV2[] Resolvers { get; }
		public CollectionResolverDefinitionV2[] CollectionResolvers { get; }
		public InjectionContext Injection { get; }
		public string PropertyName { get; }

		public string InterfaceName => InterfaceNameInfo.Name;
		public string ParameterName => PropertyName.ToLowerCamelCase();

		public CaptureDefinitionV2(TypeName interfaceNameInfo,
			string propertyName,
			ResolverDefinitionV2[] resolvers,
			CollectionResolverDefinitionV2[] collectionResolvers)
		{
			InterfaceNameInfo = interfaceNameInfo;
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
				Injection[resolver.ReturnType] = $"{propertyName}.{resolver.MethodName}({resolver.GetArgListForSelf(Injection)})";
			}
		}
	}
}
