using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Generator.Definition;
using Deptorygen.Generator.Interfaces;
using Deptorygen.Utilities;

namespace Deptorygen.Generator.Injection
{
	public class InjectionAggregator
	{
		public IEnumerable<InjectionExpression> CapabilitiesFromResolver(TypeName typeName, FactoryDefinition factory, IResolverContext caller)
		{
			foreach (var capability in factory.GetInjectionCapabilities(typeName, caller))
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
	}
}
