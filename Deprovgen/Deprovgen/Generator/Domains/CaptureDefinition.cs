using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Deprovgen.Generator.Domains
{
	public class CaptureDefinition
	{
		public CaptureDefinition(string ns, string interfaceName, string propertyName, string parameterName, ResolverDefinition[] resolvers, Accessibility accessibility)
		{
			Namespace = ns;
			InterfaceName = interfaceName;
			PropertyName = propertyName;
			ParameterName = parameterName;
			Resolvers = resolvers;
			Accessibility = accessibility;
		}

		public string Namespace { get; }
		public string InterfaceName { get; }
		public string PropertyName { get; }
		public string ParameterName { get; }
		public Accessibility Accessibility { get; set; }
		public ResolverDefinition[] Resolvers { get; }

		public static (CaptureDefinition, ResolverDefinition) GetArgExpression(CaptureDefinition[] captures, string typeName)
		{
			var resolvers = from capture in captures
				from resolver in capture.Resolvers
				where resolver.ServiceType.TypeName == typeName
				select (capture, resolver);

			return resolvers.FirstOrDefault();
		}
	}
}
