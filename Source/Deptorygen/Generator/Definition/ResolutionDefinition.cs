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
			var args = new List<string>();
			foreach (var dependency in Dependencies)
			{
				var capabilities = resolver.GetInjectionCapabilities(dependency, factory)
					.GroupBy(x => x.Type)
					.Select(x => x.OrderBy(y => y.Method).First())
					.ToDictionary(x => x.Type, x => x);

				args.Add(capabilities.TryGetValue(dependency, out var exp) ? exp.Code : "<error>");
			}

			return $"new {TypeName.Name}({args.Join(", ")})";
		}
	}
}
