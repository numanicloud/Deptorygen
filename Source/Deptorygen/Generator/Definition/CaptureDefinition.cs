using System.Collections.Generic;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class CaptureDefinition : IDefinitionRequiringNamespace, IAccessibilityClaimer
	{
		public TypeName InterfaceNameInfo { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public string PropertyName { get; }

		public string InterfaceName => InterfaceNameInfo.Name;
		public string ParameterName => PropertyName.ToLowerCamelCase();

		public CaptureDefinition(TypeName interfaceNameInfo,
			string propertyName,
			ResolverDefinition[] resolvers,
			CollectionResolverDefinition[] collectionResolvers)
		{
			InterfaceNameInfo = interfaceNameInfo;
			PropertyName = propertyName;
			Resolvers = resolvers;
			CollectionResolvers = collectionResolvers;
		}

		public IEnumerable<string> GetRequiredNamespaces()
		{
			yield return InterfaceNameInfo.FullNamespace;
		}

		public IEnumerable<Accessibility> Accessibilities
		{
			get
			{
				yield return InterfaceNameInfo.Accessibility;
			}
		}
	}
}
