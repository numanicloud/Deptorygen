using System.Collections.Generic;
using Deptorygen.Generator.Injection;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Definition
{
	public class ResolutionDefinition
	{
		public TypeName TargetType { get; }
		public TypeName TypeName { get; }
		public TypeName[] Dependencies { get; }
		public bool IsDisposable { get; }

		public ResolutionDefinition(TypeName targetType, TypeName typeName, TypeName[] dependencies, bool isDisposable)
		{
			TargetType = targetType;
			TypeName = typeName;
			Dependencies = dependencies;
			IsDisposable = isDisposable;
		}

		public string GetInstantiation(ResolverDefinition resolver, FactoryDefinition factory)
		{
			var aggregator = new InjectionAggregator(TargetType, factory, resolver);
			if (aggregator.GetPriorInjectionExpression(InjectionMethod.Resolver) is {} selfCapacity)
			{
				return selfCapacity;
			}

			var args = new List<string>();
			foreach (var dependency in Dependencies)
			{
				var aggregator1 = new InjectionAggregator(dependency, factory, resolver);
				args.Add(aggregator1.GetPriorInjectionExpression() ?? "<error>");
			}

			return $"new {TypeName.Name}({args.Join(", ")})";
		}
	}
}
