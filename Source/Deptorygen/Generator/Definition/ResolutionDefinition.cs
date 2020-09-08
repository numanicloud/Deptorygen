using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
			var selfCapability = factory.GetInjectionCapabilities(TargetType)
				.Where(x => x.Method != InjectionMethod.Resolver)
				.OrderBy(x => x.Method)
				.FirstOrDefault();
			if (selfCapability is {})
			{
				return selfCapability.Code;
			}

			var args = new List<string>();
			foreach (var dependency in Dependencies)
			{
				var capabilities = resolver.GetInjectionCapabilities(dependency, factory)
					.OrderBy(x => x.Method)
					.FirstOrDefault();

				args.Add(capabilities?.Code ?? "<error>");
			}

			return $"new {TypeName.Name}({args.Join(", ")})";
		}
	}
}
