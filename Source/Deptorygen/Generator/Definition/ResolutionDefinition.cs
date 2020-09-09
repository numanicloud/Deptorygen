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
			var aggregator = new InjectionAggregator(factory, resolver);
			return aggregator.GetResolution(this);
		}
	}
}
