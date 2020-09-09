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
		private readonly TypeName _targetType;
		private readonly FactoryDefinition _factory;
		private readonly IResolverContext _caller;

		public InjectionAggregator(TypeName targetType, FactoryDefinition factory, IResolverContext caller)
		{
			_factory = factory;
			_caller = caller;
			_targetType = targetType;
		}

		public string GetResolutionList(CollectionResolverDefinition resolver)
		{
			return resolver.ServiceTypes.Select(x => CapabilitiesFromFactory(x)
					.OrderBy(y => y.Method)
					.FirstOrDefault()?.Code ?? "<error>")
				.Join("," + Environment.NewLine + "\t\t\t\t");
		}

		public string? GetPriorInjectionExpression(params InjectionMethod[] methodsToExclude)
		{
			return CapabilitiesFromResolver()
				.Where(x => methodsToExclude.All(y => y != x.Method))
				.OrderBy(x => x.Method)
				.FirstOrDefault()?.Code;
		}

		private IEnumerable<InjectionExpression> CapabilitiesFromResolver()
		{
			foreach (var capability in CapabilitiesFromFactory(_targetType))
			{
				yield return capability;
			}

			foreach (var parameter in _caller.Parameters)
			{
				if (parameter.TypeNameInfo == _targetType)
				{
					yield return new InjectionExpression(_targetType, InjectionMethod.Parameter, parameter.VarName);
				}
			}
		}

		private IEnumerable<InjectionExpression> CapabilitiesFromFactory(TypeName typeName)
		{
			if (typeName == _factory.InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.This, "this");
			}

			var resolvers = _factory.Resolvers.Select(x => DelegationFromResolver(typeName, x))
				.FilterNull();
			var collections = _factory.CollectionResolvers.Select(x => DelegationFromResolver(typeName, x))
				.FilterNull();
			var captures = _factory.Captures.SelectMany(x => DelegationsFromCapture(typeName, x));
			var fields = _factory.Dependencies.Where(x => x == typeName)
				.Select(x => new InjectionExpression(typeName, InjectionMethod.Field, $"_{x.LowerCamelCase}"));

			foreach (var expression in resolvers.Concat(collections).Concat(captures).Concat(fields))
			{
				yield return expression;
			}
		}

		private IEnumerable<InjectionExpression> DelegationsFromCapture(TypeName typeName, CaptureDefinition capture)
		{
			if (typeName == capture.InterfaceNameInfo)
			{
				yield return new InjectionExpression(typeName, InjectionMethod.CapturedFactory, capture.PropertyName);
			}

			var capabilities1 = capture.Resolvers.Select(x => DelegationFromResolver(typeName, x));
			var capabilities2 = capture.CollectionResolvers.Select(x => DelegationFromResolver(typeName, x));
			foreach (var expression in capabilities1.Concat(capabilities2).FilterNull())
			{
				yield return new InjectionExpression(
					typeName,
					InjectionMethod.CapturedResolver,
					$"{capture.PropertyName}.{expression.Code}");
			}
		}

		private InjectionExpression? DelegationFromResolver(TypeName typeName, IResolverContext resolver)
		{
			if (typeName != resolver.ReturnType) return null;

			var args = resolver.Parameters
				.Select(x => _caller.GetPriorInjectionExpression(x.TypeNameInfo, _factory) ?? "<error>")
				.Join(", ");

			return new InjectionExpression(typeName,
				InjectionMethod.Resolver,
				$"{resolver.MethodName}({args})");
		}
	}
}
