using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Injection;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Interfaces
{
	public interface IResolverContext
	{
		TypeName ReturnType { get; }
		VariableDefinition[] Parameters { get; }
		IEnumerable<InjectionExpression> GetInjectionCapabilities(TypeName typeName, FactoryDefinition factory);
	}

	public static class ResolverContextExtensions
	{
		public static string? GetPriorInjectionExpression(this IResolverContext resolver,
			TypeName typeName,
			FactoryDefinition factory,
			params InjectionMethod[] methodsToExclude)
		{
			var aggregator = new InjectionAggregator();

			return aggregator.CapabilitiesFromResolver(typeName, factory, resolver)
				.Where(x => methodsToExclude.All(y => y != x.Method))
				.OrderBy(x => x.Method)
				.FirstOrDefault()?.Code;
		}
	}
}
