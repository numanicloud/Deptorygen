using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deptorygen.Generator.Definition;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Interfaces
{
	public interface IResolverContext
	{
		TypeName ReturnType { get; }
		IEnumerable<InjectionExpression> GetInjectionCapabilities(TypeName typeName, FactoryDefinition factory);
	}

	public static class ResolverContextExtensions
	{
		public static string? GetPriorInjectionExpression(this IResolverContext resolver,
			TypeName typeName,
			FactoryDefinition factory,
			params InjectionMethod[] methodsToExclude)
		{
			return resolver.GetInjectionCapabilities(typeName, factory)
				.Where(x => methodsToExclude.All(y => y != x.Method))
				.OrderBy(x => x.Method)
				.FirstOrDefault()?.Code;
		}
	}
}
