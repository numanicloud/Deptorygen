using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Injection
{
	public class InjectionAggregator
	{
		public string GetResolutionList(FactoryDefinition factory, CollectionResolverDefinition resolver)
		{
			return resolver.ServiceTypes.Select(x => CapabilitiesFromFactory(x, factory, resolver)
					.OrderBy(y => y.Method)
					.FirstOrDefault()?.Code ?? "<error>")
				.Join("," + Environment.NewLine + "\t\t\t\t");
		}

		public string? GetPriorInjectionExpression(TypeName typeName,
			FactoryDefinition factory,
			IResolverContext resolver,
			params InjectionMethod[] methodsToExclude)
		{
			return CapabilitiesFromResolver(typeName, factory, resolver)
				.Where(x => methodsToExclude.All(y => y != x.Method))
				.OrderBy(x => x.Method)
				.FirstOrDefault()?.Code;
		}

		private IEnumerable<InjectionExpression> CapabilitiesFromResolver(TypeName typeName, FactoryDefinition factory, IResolverContext caller)
		{
			foreach (var capability in CapabilitiesFromFactory(typeName, factory, caller))
			{
				yield return capability;
			}

			foreach (var parameter in caller.Parameters)
			{
				if (parameter.TypeNameInfo == typeName)
				{
					yield return new InjectionExpression(typeName, InjectionMethod.Parameter, parameter.VarName);
				}
			}
		}

		private IEnumerable<InjectionExpression> CapabilitiesFromFactory(TypeName typeName, FactoryDefinition factory, IResolverContext caller)
		{
			if (typeName == factory.InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.This, "this");
			}

			var resolvers = factory.Resolvers.Select(x => x.GetDelegation(typeName, factory, caller)).FilterNull();
			var collections = factory.CollectionResolvers.Select(x => x.GetDelegations(typeName, factory)).FilterNull();
			var captures = factory.Captures.SelectMany(x => DelegationsFromCapture(typeName, factory, x, caller));
			var fields = factory.Dependencies.Where(x => x == typeName)
				.Select(x => new InjectionExpression(typeName, InjectionMethod.Field, $"_{x.LowerCamelCase}"));

			foreach (var expression in resolvers.Concat(collections).Concat(captures).Concat(fields))
			{
				yield return expression;
			}
		}
		
		public IEnumerable<InjectionExpression> DelegationsFromCapture(TypeName typeName, FactoryDefinition factory, CaptureDefinition capture, IResolverContext caller)
		{
			if (typeName == capture.InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.CapturedFactory, capture.PropertyName);
			}

			var capabilities1 = capture.Resolvers.Select(x => x.GetDelegation(typeName, factory, caller));
			var capabilities2 = capture.CollectionResolvers.Select(x => x.GetDelegations(typeName, factory));
			foreach (var expression in capabilities1.Concat(capabilities2).FilterNull())
			{
				yield return new InjectionExpression(
					typeName,
					InjectionMethod.CapturedResolver,
					$"{capture.PropertyName}.{expression.Code}");
			}
		}
	}
}
