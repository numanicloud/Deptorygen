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
		string MethodName { get; }
		TypeName ReturnType { get; }
	}

	public static class ResolverContextExtensions
	{
		public static string? GetPriorInjectionExpression(this IResolverContext caller,
			TypeName typeName,
			FactoryDefinition factory,
			params InjectionMethod[] methodsToExclude)
		{
			var aggregator = new InjectionAggregator(factory, caller);
			return aggregator.GetPriorInjectionExpression(typeName, methodsToExclude);
		}
	}
}
