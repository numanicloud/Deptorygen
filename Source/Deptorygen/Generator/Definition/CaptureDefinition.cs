using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;
using Microsoft.CodeAnalysis;

namespace Deptorygen.Generator.Definition
{
	public class CaptureDefinition : IDefinitionRequiringNamespace, IInjectionGenerator, IAccessibilityClaimer
	{
		public TypeName InterfaceNameInfo { get; }
		public ResolverDefinition[] Resolvers { get; }
		public CollectionResolverDefinition[] CollectionResolvers { get; }
		public InjectionContext Injection { get; }
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

			var generators = Resolvers.Cast<IInjectionGenerator>()
				.Concat(CollectionResolvers)
				.ToArray();
			Injection = new InjectionContext(generators);
		}

		public IEnumerable<string> GetRequiredNamespaces()
		{
			yield return InterfaceNameInfo.FullNamespace;
		}

		public string? GetInjectionExpression(ResolverDefinition resolver, InjectionContext context)
		{
			var resolvers = Resolvers.Where(x => x.IsTransient == resolver.IsTransient)
				.Cast<IInjectionGenerator>();
			return GetInjectionExpression(resolver.ReturnType, context, resolvers);
		}

		public string? GetInjectionExpression(TypeName typeName, InjectionContext context)
		{
			var resolvers = Resolvers.Cast<IInjectionGenerator>()
				.Concat(CollectionResolvers);
			return GetInjectionExpression(typeName, context, resolvers);
		}

		public IEnumerable<InjectionExpression> GetInjectionExpressions(TypeName typeName, InjectionContext context)
		{
			IEnumerable<InjectionExpression> Enumerate(IEnumerable<IInjectionGenerator> generators)
			{
				foreach (var generator in generators)
				{
					foreach (var expression in generator.GetInjectionExpressions(typeName, context))
					{
						yield return new InjectionExpression(expression.Type, InjectionMethod.CapturedResolver, expression.Code);
					}
				}
			}

			foreach (var exp in Enumerate(Resolvers))
			{
				yield return exp;
			}

			foreach (var exp in Enumerate(CollectionResolvers))
			{
				yield return exp;
			}
		}

		private string? GetInjectionExpression(TypeName typeName, InjectionContext context,
			IEnumerable<IInjectionGenerator> generators)
		{
			var code = generators
				.Select(x => x.GetInjectionExpression(typeName, context))
				.FilterNull()
				.FirstOrDefault();
			return code is null ? null : $"{PropertyName}.{code}";
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
