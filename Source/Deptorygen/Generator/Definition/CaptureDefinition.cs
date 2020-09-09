using System.Collections.Generic;
using System.Linq;
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

		public IEnumerable<InjectionExpression> GetDelegations(TypeName typeName, FactoryDefinition factory, IResolverContext caller)
		{
			if (typeName == InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.CapturedFactory, PropertyName);
			}

			var capabilities1 = Resolvers.Select(x => x.GetDelegation(typeName, factory, caller));
			var capabilities2 = CollectionResolvers.Select(x => x.GetDelegations(typeName, factory));
			foreach (var expression in capabilities1.Concat(capabilities2).FilterNull())
			{
				yield return new InjectionExpression(
					typeName,
					InjectionMethod.CapturedResolver,
					$"{PropertyName}.{expression.Code}");
			}
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
