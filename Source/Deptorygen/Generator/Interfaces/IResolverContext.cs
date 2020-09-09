using System.Collections;
using System.Linq;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Injection;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Interfaces
{
	public interface IResolverContext
	{
		VariableDefinition[] Parameters { get; }
	}

	public static class ResolverContextExtensions
	{
		public static string? GetPriorInjectionExpression(this IResolverContext resolver,
			TypeName typeName,
			FactoryDefinition factory,
			params InjectionMethod[] methodsToExclude)
		{
			var aggregator = new InjectionAggregator();
			return aggregator.GetPriorInjectionExpression(typeName, factory, resolver, methodsToExclude);
		}
	}
}
